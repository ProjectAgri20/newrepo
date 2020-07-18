using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.Framework.Assets
{
    /// A read-only collection of <see cref="ExternalCredentialInfo" /> objects.
    public sealed class ExternalCredentialInfoCollection : ReadOnlyCollection<ExternalCredentialInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalCredentialInfoCollection"/> class.
        /// </summary>
        /// <param name="externalCredentials">The external credentials to include in the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="externalCredentials" /> is null.</exception>
        public ExternalCredentialInfoCollection(IList<ExternalCredentialInfo> externalCredentials)
            :base(externalCredentials)
        {
        }
    }
}
