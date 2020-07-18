using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.GFriendExecution
{
    [DataContract]
    internal class GFriendExecutionActivityData
    {
        /// <summary>
        /// List of files which are used for executing GFriend Scripts (Script file, variable file and library file)
        /// </summary>
        [DataMember]
        public List<GFriendFile> GFriendFiles { get; set; }

        /// <summary>
        /// Gets or sets the LockTimeOut Data: Acquire Lock Timeouts and Hold Lock Timeouts
        /// </summary>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }
        public GFriendExecutionActivityData()
        {
            GFriendFiles = new List<GFriendFile>();
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
        }
    }
}
