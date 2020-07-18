using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// DataLog table, OfficeWorkerPerformance, properties
    /// </summary>
    public class VirtualResourceInstanceStatusLogger : FrameworkDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "VirtualResourceInstanceStatus";
        /// <summary>
        /// The name of the property representing hte primary key for the table
        /// </summary>
        public override string PrimaryKeyColumn => nameof(VirtualResourceStatusId);

        /// <summary>
        /// Gets the virtual resource ID
        /// </summary>
        [DataLogProperty]
        public Guid VirtualResourceStatusId { get; set; }
        
        /// <summary> 
        /// Gets the Session ID
        /// </summary>
        [DataLogProperty]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets the username of the worker being tracked.
        /// </summary>
        [DataLogProperty]
        public string UserName { get; set; }

        /// <summary>
        /// Time of change in status
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset TimeStamp { get; set; }

        /// <summary>
        /// Index (order) of change being tracked.
        /// </summary>
        [DataLogProperty]
        public int Index { get; set; }

        /// <summary>
        /// Status that the change is transitioning to
        /// </summary>
        [DataLogProperty]
        public string TransitionTo { get; set; }

        /// <summary>
        /// Gets whether we are in an idle or running status.
        /// </summary>
        [DataLogProperty]
        public bool TransitionActive {get;set;}

        /// <summary>
        /// Gets whether we are in an idle or running status.
        /// </summary>
        [DataLogProperty]
        public string Caller { get; set; }

        /// <summary>
        /// Resource instance Id
        /// </summary>
        [DataLogProperty]
        public string ResourceInstanceId { get; set; }

        /// <summary>
        /// Contructor for VirtualResourceInstanceStatusLogger entry
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="userName"></param>
        /// <param name="index"></param>
        /// <param name="transitionTo"></param>
        /// <param name="status"></param>
        /// <param name="resourceInstanceId"></param>
        public VirtualResourceInstanceStatusLogger(string sessionId, string userName, int index,  string transitionTo, bool status, string resourceInstanceId)
        {
            VirtualResourceStatusId = SequentialGuid.NewGuid();
            SessionId = sessionId;
            UserName = userName;
            TimeStamp = DateTime.Now;
            Index = index;
            TransitionTo = transitionTo;
            TransitionActive = status;
            ResourceInstanceId = resourceInstanceId;
        }
        /// <summary>
        /// Updates fields for Virtual ResourceInstanceLogger class
        /// </summary>
        /// <param name="index"></param>
        /// <param name="transitionTo"></param>
        /// <param name="status"></param>
        /// <param name="caller"></param>
        public void Update( int index, string transitionTo, bool status, string caller)
        {
            VirtualResourceStatusId = SequentialGuid.NewGuid();
            TimeStamp = DateTime.Now;
            Index = index;
            TransitionTo = transitionTo;
            TransitionActive = status;
            Caller = caller;
        }
    }
}
