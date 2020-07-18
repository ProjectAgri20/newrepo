using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Manifest details for a base virtual resource
    /// </summary>
    [DataContract]
    public class VirtualResourceDetail : ResourceDetailBase
    {
        /// <summary>
        /// Gets the unique names associated with the specific resource type.
        /// </summary>
        public override IEnumerable<string> UniqueNames
        {
            get { yield return base.Name; }
        }
    }
}
