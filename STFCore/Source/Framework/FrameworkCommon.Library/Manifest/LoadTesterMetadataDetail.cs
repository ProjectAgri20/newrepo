using System;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    [KnownType(typeof(LoadTesterExecutionPlan))]
    [DataContract]
    public class LoadTesterMetadataDetail : ResourceMetadataDetail
    {
        private LoadTesterExecutionPlan _plan = new LoadTesterExecutionPlan();
        /// <summary>
        /// Gets or sets the exection plan.
        /// </summary>
        [DataMember]
        public override IActivityExecutionPlan Plan
        {
            get { return _plan; }
            set { _plan = (LoadTesterExecutionPlan)value; }
        }
    }
}
