using System;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// Represents an SSPI security context.
    /// </summary>
    public abstract class SecurityContext : IDisposable
    {
        private readonly SafeContextHandle _safeContextHandle;
        private readonly Lazy<string> _contextUserName;
        private readonly Lazy<string> _authorityName;

        /// <summary>
        /// Gets a value indicating whether this <see cref="SecurityContext" /> is fully formed.
        /// </summary>
        public bool Initialized { get; protected set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SecurityContext" /> has been disposed.
        /// </summary>
        protected bool Disposed { get; private set; } = false;

        /// <summary>
        /// Gets the logon user name that the context represents.
        /// </summary>
        public string ContextUserName
        {
            get
            {
                CheckLifecycle();
                return _contextUserName.Value;
            }
        }

        /// <summary>
        /// Gets the name of the authenticating authority for the context.
        /// </summary>
        public string AuthorityName
        {
            get
            {
                CheckLifecycle();
                return _authorityName.Value;
            }
        }

        /// <summary>
        /// Gets the handle to the context.
        /// </summary>
        internal SafeContextHandle Handle
        {
            get
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException(nameof(SecurityContext));
                }
                return _safeContextHandle;
            }
        }

        /// <summary>
        /// The credential being used by this context to authenticate itself to other actors.
        /// </summary>
        protected SecurityCredential Credential { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityContext" /> class.
        /// </summary>
        protected SecurityContext(SecurityCredential credential)
        {
            Credential = credential;
            _safeContextHandle = new SafeContextHandle();

            _authorityName = new Lazy<string>(() => _safeContextHandle.GetAuthorityName());
            _contextUserName = new Lazy<string>(() => _safeContextHandle.GetContextUserName());
        }

        private void CheckLifecycle()
        {
            if (Initialized == false)
            {
                throw new SspiException("The context is not yet fully formed.");
            }
            else if (Disposed)
            {
                throw new ObjectDisposedException(nameof(SecurityContext));
            }
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
            if (!Disposed)
            {
                if (disposing)
                {
                    _safeContextHandle.Dispose();
                }
                Disposed = true;
            }
        }

        #endregion
    }
}
