using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Manifest details for the Admin Worker virtual resource
    /// </summary>
    [DataContract]
    public class AdminWorkerDetail : OfficeWorkerDetail
    {
        /// <summary>
        /// Gets the unique names associated with the specific resource type.
        /// </summary>
        public override IEnumerable<string> UniqueNames
        {
            get { return UserCredentials.Select(x => x.ResourceInstanceId); }
        }
    }
}