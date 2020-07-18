using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Class for handling runtime errors
    /// </summary>
    [DataContract]
    public class RuntimeError
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public RuntimeError(string name)
        {
            ElementName = name;
            ErrorId = SequentialGuid.NewGuid();
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ElementName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Guid ErrorId { get; set; }
    }
}
 