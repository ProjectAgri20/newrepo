using System;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// Provides access to the pre-existing credentials of a security principle.
    /// </summary>
    public abstract class SecurityCredential : IDisposable
    {
        private bool _disposed = false;
        private readonly SafeCredentialHandle _safeCredentialHandle;
        private readonly Lazy<string> _principleName;

        /// <summary>
        /// Gets information about the security package for this credential.
        /// </summary>
        public SecurityPackageInfo PackageInfo { get; }

        /// <summary>
        /// Gets the User Principle Name of this credential.
        /// </summary>
        public string PrincipleName => _principleName.Value;

        /// <summary>
        /// Gets the handle to the credential.
        /// </summary>
        internal SafeCredentialHandle Handle
        {
            get
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(SecurityCredential));
                }
                return _safeCredentialHandle;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityCredential" /> class.
        /// </summary>
        /// <param name="package">The security package to use.</param>
        /// <param name="use">The manner in which the credential will be used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="package" /> is null.</exception>
        protected SecurityCredential(SecurityPackageInfo package, CredentialUse use)
        {
            PackageInfo = package ?? throw new ArgumentNullException(nameof(package));

            _safeCredentialHandle = new SafeCredentialHandle();
            _safeCredentialHandle.AcquireCredentialHandle(package.Name, use);

            _principleName = new Lazy<string>(() => _safeCredentialHandle.GetPrincipleName());
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _safeCredentialHandle.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
