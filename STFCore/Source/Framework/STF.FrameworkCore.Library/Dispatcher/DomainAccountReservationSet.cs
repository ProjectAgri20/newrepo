using System;
using System.Collections;
using System.Collections.Generic;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework.Settings;
using System.Collections.ObjectModel;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// A collection of domain account reservations used by any resource that needs unique user accounts.
    /// </summary>
    public class DomainAccountReservationSet
    {
        private readonly Dictionary<string, DomainAccount> _accounts = new Dictionary<string, DomainAccount>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainAccountReservationSet"/> class.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public DomainAccountReservationSet(string sessionId)
        {
            SessionId = sessionId;
        }

        /// <summary>
        /// Gets the session id.
        /// </summary>
        /// <value>
        /// The session id.
        /// </value>
        public string SessionId { get; private set; }

        /// <summary>
        /// Gets the user pool values for this reservation set.
        /// </summary>
        /// <value>The user pools.</value>
        public ArrayList UserPools
        {
            get  { return new ArrayList(_accounts.Keys); }
        }

        /// <summary>
        /// Totals the number of accounts reserved for the defined account type.
        /// </summary>
        /// <param name="userPool">The account pool type.</param>
        /// <returns>An <see cref="System.Int32"/> value defining the number of accounts reserved.</returns>
        public int TotalReserved(string userPool)
        {
            return _accounts.ContainsKey(userPool) ? _accounts[userPool].TotalAccounts : 0;
        }

        /// <summary>
        /// Creates a reservation entry for the specified account pool.
        /// </summary>
        /// <param name="userPool">The account pool type.</param>
        /// <param name="accountPool">The account pool.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="userCount">The number of user accounts to reserve.</param>
        public void Add(string userPool, DomainAccountPool accountPool, int startIndex, int userCount)
        {
            _accounts.Add(userPool, new DomainAccount(startIndex, userCount, accountPool));
        }

        /// <summary>
        /// Gets the next available domain user for the specified key.
        /// </summary>
        /// <param name="poolType">The account key.</param>
        /// <returns></returns>
        public string NextUserName(string poolType)
        {
            return _accounts[poolType].GetNextUserName().Item1;
        }

        /// <summary>
        /// Gets the next available domain user.
        /// </summary>
        /// <param name="userPool">The account key.</param>
        /// <returns></returns>
        public OfficeWorkerCredential NextUserCredential(string userPool)
        {
            Tuple<string, int> username = _accounts[userPool].GetNextUserName();
            string officeWorkerDomain = GlobalSettings.Items[Setting.Domain];
            string officeWorkerPassword = GlobalSettings.Items[Setting.OfficeWorkerPassword];

            return new OfficeWorkerCredential(username.Item1, officeWorkerPassword, officeWorkerDomain, username.Item2);
        }

        /// <summary>
        /// Gets the start index for the specified key.
        /// </summary>
        /// <param name="userPool">The account key.</param>
        /// <returns></returns>
        public int StartIndex(string userPool)
        {
            return _accounts.ContainsKey(userPool) ? _accounts[userPool].StartIndex : 0;
        }

        /// <summary>
        /// Returns the user format for the User Pool
        /// </summary>
        /// <param name="userPool">The account key.</param>
        /// <returns></returns>
        public string UserFormat(string userPool)
        {
            return _accounts.ContainsKey(userPool) ? _accounts[userPool].UserNameFormat : string.Empty;
        }

        private class DomainAccount
        {
            private int _currentIndex;
            private int _userCount;
            private readonly DomainAccountPool _accountPool;

            public int StartIndex { get; private set; }

            public DomainAccount(int startIndex, int userCount, DomainAccountPool accountPool)
            {
                TotalAccounts = _userCount = userCount;
                StartIndex = _currentIndex = startIndex;
                _accountPool = accountPool;
            }

            public int TotalAccounts { get; private set; }

            public string UserNameFormat
            {
                get { return _accountPool.UserNameFormat; }
            }

            public Tuple<string, int> GetNextUserName()
            {
                if ((_userCount <= 0))
                {
                    throw new InvalidOperationException("No more users available to allocate.");
                }

                _userCount--;

                Tuple<string, int> userName = new Tuple<string, int>
                    (
                        string.Format(_accountPool.UserNameFormat, _currentIndex),
                        _currentIndex
                    );
                _currentIndex++;

                return userName;
            }
        }
    }
}
