using System;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.EdtIntervention
{
    public class FaultEventDataLog : ActivityDataLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultEventDataLog"/> class.
        /// </summary>
        public FaultEventDataLog(PluginExecutionData executionData)
            : base(executionData)
        {
            Message = executionData.GetMetadata<EdtInterventionActivityData>().AlertMessage;
        }

        /// <summary>
        /// Gets name of the table
        /// </summary>
        public override string TableName => "FaultEvents";

        [DataLogProperty(MaxLength = -1)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the Event Type Id
        /// </summary>
        [DataLogProperty(MaxLength = 36)]
        public Guid EventTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Event SubType Id
        /// </summary>
        [DataLogProperty(MaxLength = 36)]
        public Guid EventSubTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Op In Progress Id
        /// </summary>
        [DataLogProperty(MaxLength = 36)]
        public Guid OpInProgressId { get; set; }

        /// <summary>
        /// Gets or sets the Recovery Id
        /// </summary>
        [DataLogProperty(MaxLength = 36)]
        public Guid RecoveryId { get; set; }

        /// <summary>
        /// Gets or sets the Job Disposition Id
        /// </summary>
        [DataLogProperty(MaxLength = 36)]
        public Guid JobDispositionId { get; set; }

        /// <summary>
        /// Gets or sets the Root Cause Id
        /// </summary>
        [DataLogProperty(MaxLength = 36)]
        public Guid RootCauseId { get; set; }

        /// <summary>
        /// Gets or sets the Event Time
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset EventTime { get; set; }

        /// <summary>
        /// Gets or sets the Event Details
        /// </summary>
        [DataLogProperty(MaxLength = -1)]
        public string EventDetails { get; set; }

        /// <summary>
        /// Gets or sets the Recovery Time
        /// </summary>
        [DataLogProperty]
        public int RecoveryTime { get; set; }

        /// <summary>
        /// Gets or sets the Fault Code
        /// </summary>
        [DataLogProperty(MaxLength = 36)]
        public string FaultCode { get; set; }
        

        ///// <summary>
        ///// Gets or sets the Logs Gathered
        ///// </summary
        //[DataLogProperty(MaxLength = -1)]
        //public string LogsGathered { get; set; }
        
        /// <summary>
        /// Gets or sets the Link Status indicating whether the current event is linked to the previous event
        /// </summary>
        [DataLogProperty]
        public bool IsLinked { get; set; }
    }
}
