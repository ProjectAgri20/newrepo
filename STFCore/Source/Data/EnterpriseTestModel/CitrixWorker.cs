using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    [ObjectFactory(VirtualResourceType.CitrixWorker)]
    public partial class CitrixWorker
    {
        public CitrixWorker()
            : this("CitrixWorker")
        {
        }

        public CitrixWorker(string resourceType)
            : base(resourceType)
        {
            DBWorkerRunMode = "PublishedApp";
            ServerHostname = string.Empty;
            PublishedApp = string.Empty;
        }

        protected override void LoadChildDetail(ResourceDetailBase detail)
        {
            base.LoadChildDetail(detail);

            CitrixWorkerDetail resourceDetail = detail as CitrixWorkerDetail;
            if (resourceDetail != null)
            {
                PublishedApp = resourceDetail.PublishedApp;
                RunMode = resourceDetail.WorkerRunMode;
                ServerHostname = resourceDetail.ServerHostname;
            }
        }

        public override void CopyResourceProperties(VirtualResource resource)
        {
            base.CopyResourceProperties(resource);

            CitrixWorker worker = resource as CitrixWorker;
            if (worker != null)
            {
                this.ServerHostname = worker.ServerHostname;
                this.DBWorkerRunMode = worker.DBWorkerRunMode;
                this.PublishedApp = worker.PublishedApp;
            }
            else
            {
                throw new ArgumentException("Resource must be of type CitrixWorker.", "resource");
            }
        }

        public CitrixWorkerRunMode RunMode
        {
            get { return (CitrixWorkerRunMode)Enum.Parse(typeof(CitrixWorkerRunMode), DBWorkerRunMode); }
            set { DBWorkerRunMode = value.ToString(); }
        }

        public override IEnumerable<string> ValidateData()
        {
            if (RunMode == CitrixWorkerRunMode.None)
            {
                yield break; 
            }
            else
            {
                base.ValidateData();
            }
        }
    }
}
