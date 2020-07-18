using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Manifest details for driver versions
    /// </summary>
    [DataContract]
    public class DriverVersionDetail
    {
        [DataMember]
        private Collection<int> _versionData = new Collection<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverVersionDetail"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public DriverVersionDetail(Collection<int> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            foreach (var item in data)
            {
                _versionData.Add(item);
            }
        }

        /// <summary>
        /// Gets the version data as a collection of integers.
        /// </summary>
        public Collection<int> Data
        {
            get { return _versionData; }
        }
    }
}
