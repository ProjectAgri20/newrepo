using System;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    [ObjectFactory(VirtualResourceType.AdminWorker)]
    public partial class AdminWorker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminWorker"/> class.
        /// </summary>
        public AdminWorker()
            : this("AdminWorker")
        {
        }

        public AdminWorker(string resourceType)
            : base(resourceType)
        {
            ExecutionMode = ExecutionMode.Iteration;
        }

        protected override void LoadChildDetail(ResourceDetailBase detail)
        {
            var resourceDetail = detail as AdminWorkerDetail;
            ExecutionMode = resourceDetail.ExecutionMode;
            Enabled = resourceDetail.Enabled;
        }

        /// <summary>
        /// Copies all relevant metadata from the target VirtualResource to this instance.
        /// </summary>
        /// <param name="resource">The resource to copy from.</param>
        public override void CopyResourceProperties(VirtualResource resource)
        {
            base.CopyResourceProperties(resource);
            AdminWorker adminWorker = resource as AdminWorker;
            if (adminWorker != null)
            {
                DBExecutionMode = adminWorker.DBExecutionMode;
            }
            else
            {
                throw new ArgumentException("Resource must be of type AdminWorker.", "resource");
            }
        }

        /// <summary>
        /// Gets or sets the execution mode.
        /// </summary>
        public ExecutionMode ExecutionMode
        {
            get { return EnumUtil.Parse<ExecutionMode>(DBExecutionMode); }
            set { DBExecutionMode = value.ToString(); }
        }
    }
}
