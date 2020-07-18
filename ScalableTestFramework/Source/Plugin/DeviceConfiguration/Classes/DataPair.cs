using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DeviceConfiguration.Classes
{

    /// <summary>
    /// Generic Data pair used for deciding what data in the plugin to set.
    /// Enables us to choose what information to actually set in a run without deleting data in configuration
    /// Ex. We have a setting which we do not want to set in the plugin, but has a value in configuration, we simply turn it off (false) for execution.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    [DataContract]
    public class DataPair<TKey>
    {

        /// <summary>
        /// Data value that would be changed/activated
        /// </summary>
        [DataMember]
        public TKey Key { get; set; }

        /// <summary>
        /// Determines whether the value is used in execution
        /// </summary>
        [DataMember]
        public bool Value { get; set; }

        /// <summary>
        /// Potential deprecation.
        /// </summary>
        public Type Type { get; set; }
        public DataPair()
        {
            Value = false;
            Type = null;
        }
    }
}
