using System;
using System.Linq;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Global object that contains the Resource Dispatch information (SystemManifest) sent from the
    /// central dispatch server.
    /// </summary>
    public static class GlobalDataStore
    {
        private static SystemManifest _manifest = new SystemManifest();
        private static Guid _currentTransaction = Guid.Empty;
        private static Guid _currentActivity = Guid.Empty;
        private static OfficeWorkerCredential _credential = null;
        private static string _resourceInstanceId = string.Empty;

        /// <summary>
        /// Gets the <see cref="SystemManifest"/> used by this instance.
        /// </summary>
        /// <value>The data.</value>
        public static SystemManifest Manifest
        {
            get { return _manifest; }
        }

        /// <summary>
        /// Stores the specified <see cref="SystemManifest"/> into this instance.
        /// </summary>
        /// <param name="manifest">The dispatch.</param>
        public static void Load(SystemManifest manifest)
        {
            _manifest = manifest;
        }

        /// <summary>
        /// Stores the specified <see cref="SystemManifest"/> into this instance.
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="instanceId"></param>
        public static void Load(SystemManifest manifest, string instanceId)
        {
            _manifest = manifest;
            _resourceInstanceId = instanceId;
        }

        /// <summary>
        /// Gets the domain user credential for the current resource.
        /// </summary>
        public static OfficeWorkerCredential Credential
        {
            get
            {
                if (_credential == null)
                {
                    if (_manifest.Resources.Credentials.Count() == 0)
                    {
                        throw new InvalidOperationException("No credentials are defined in this manifest");
                    }

                    _credential = _manifest.Resources.Credentials.FirstOrDefault(x => x.ResourceInstanceId.Equals(_resourceInstanceId));
                    if (_credential == null)
                    {
                        throw new InvalidOperationException("No credentials are defined in this manifest for Id {0}".FormatWith(_resourceInstanceId));
                    }
                }

                return _credential;
            }
        }

        /// <summary>
        /// Returns the current resource instance ID.
        /// </summary>
        public static string ResourceInstanceId
        {
            get { return _resourceInstanceId; }
        }

        /// <summary>
        /// Gets or sets the current transaction.
        /// </summary>
        /// <value>
        /// The current transaction.
        /// </value>
        public static Guid CurrentTransaction
        {
            get { return _currentTransaction; }
            set { _currentTransaction = value; }
        }

        /// <summary>
        /// Gets or sets the current activity.
        /// </summary>
        /// <value>
        /// The current activity.
        /// </value>
        public static Guid CurrentActivity
        {
            get { return _currentActivity; }
            set { _currentActivity = value; }
        }
    }
}
