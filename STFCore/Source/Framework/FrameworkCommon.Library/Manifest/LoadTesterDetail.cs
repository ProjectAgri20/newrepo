using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    [DataContract]
    public class LoadTesterDetail : OfficeWorkerDetail
    {
        /// <summary>
        /// Gets or sets the maximum number of threads per VM for this resource.
        /// </summary>
        [DataMember]
        public int ThreadsPerVM { get; set; }
    }
}