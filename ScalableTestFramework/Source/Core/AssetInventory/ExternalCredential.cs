using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// An external credential associated with a domain user.
    /// </summary>
    [DebuggerDisplay("{UserName,nq}")]
    public class ExternalCredential
    {
        /// <summary>
        /// Gets or sets the unique identifier for the badge.
        /// </summary>
        public Guid ExternalCredentialId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the badge box this badge is connected to (if any).
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user name associated with this badge.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the external credential type (provider).
        /// </summary>
        public string ExternalCredentialType { get; set; }

        /// <summary>
        /// Gets or sets the username for the domain user
        /// </summary>
        public string DomainUserName { get; set; }

    }
}
