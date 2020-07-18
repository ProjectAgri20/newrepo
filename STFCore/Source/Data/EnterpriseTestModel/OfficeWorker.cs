using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Xml;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    [ObjectFactory(VirtualResourceType.OfficeWorker)]
    public partial class OfficeWorker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorker"/> class.
        /// </summary>
        public OfficeWorker()
            : this("OfficeWorker")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorker"/> class.
        /// </summary>
        public OfficeWorker(string resourceType)
            : base(resourceType)
        {
            RepeatCount = 1;
            RandomizeActivities = true;
            MinActivityDelay = 10;
            MaxActivityDelay = 20;
            RandomizeActivityDelay = false;
            MinStartupDelay = 10;
            MaxStartupDelay = 20;
            RandomizeStartupDelay = false;
            ExecutionMode = ExecutionMode.Iteration;
            DurationTime = 0;
            SecurityGroups = string.Empty;
            UserPool = "User";
        }

        protected override void LoadChildDetail(ResourceDetailBase detail)
        {
            var resourceDetail = detail as OfficeWorkerDetail;

            RandomizeActivities = resourceDetail.RandomizeActivities;
            RandomizeStartupDelay = resourceDetail.RandomizeStartupDelay;
            MinStartupDelay = resourceDetail.MinStartupDelay;
            MaxStartupDelay = resourceDetail.MaxStartupDelay;
            RandomizeActivityDelay = resourceDetail.RandomizeActivityDelay;
            MinActivityDelay = resourceDetail.MinActivityDelay;
            MaxActivityDelay = resourceDetail.MaxActivityDelay;
            RepeatCount = resourceDetail.RepeatCount;
            ExecutionMode = resourceDetail.ExecutionMode;
            Enabled = resourceDetail.Enabled;

            if (resourceDetail.ExecutionMode == Framework.ExecutionMode.Scheduled)
            {
                ExecutionSchedule = resourceDetail.ExecutionSchedule;
            }

            DurationTime = resourceDetail.DurationTime;
            SecurityGroups = resourceDetail.SecurityGroups;
            ResourcesPerVM = resourceDetail.ResourcesPerVM;
        }

        /// <summary>
        /// The expected run time of this instance.  Returns 0 if no run time is specified.
        /// </summary>
        public override TimeSpan Runtime
        {
            get
            {
                switch (ExecutionMode)
                {
                    case ExecutionMode.Scheduled:
                    case ExecutionMode.Duration:
                    case ExecutionMode.RateBased:
                    case ExecutionMode.SetPaced:
                        return TimeSpan.FromMinutes(DurationTime);
                    default:
                        return TimeSpan.Zero;
                }
            }
        }

        /// <summary>
        /// Gets the duration string.
        /// </summary>
        public override string DurationString
        {
            get
            {
                switch (ExecutionMode)
                {
                    case ExecutionMode.Iteration:
                    case ExecutionMode.RateBased:
                        return RepeatCount.ToString(CultureInfo.CurrentCulture) + " Iterations";
                    default:
                        return base.DurationString;
                }
            }
        }

        /// <summary>
        /// Copies all relevant metadata from the target VirtualResource to this instance.
        /// </summary>
        /// <param name="resource">The resource to copy from.</param>
        public override void CopyResourceProperties(VirtualResource resource)
        {
            base.CopyResourceProperties(resource);
            OfficeWorker officeWorker = resource as OfficeWorker;
            if (officeWorker != null)
            {
                this.RepeatCount = officeWorker.RepeatCount;
                this.RandomizeStartupDelay = officeWorker.RandomizeStartupDelay;
                this.MinStartupDelay = officeWorker.MinStartupDelay;
                this.MaxStartupDelay = officeWorker.MaxStartupDelay;
                this.RandomizeActivities = officeWorker.RandomizeActivities;
                this.RandomizeActivityDelay = officeWorker.RandomizeActivityDelay;
                this.MinActivityDelay = officeWorker.MinActivityDelay;
                this.MaxActivityDelay = officeWorker.MaxActivityDelay;
                this.ExecutionMode = officeWorker.ExecutionMode;
                this.DurationTime = officeWorker.DurationTime;
                this.SecurityGroups = officeWorker.SecurityGroups;
                this.ResourcesPerVM = officeWorker.ResourcesPerVM;
                this.ExecutionSchedule = officeWorker.ExecutionSchedule;
                this.UserPool = officeWorker.UserPool;
            }
            else
            {
                throw new ArgumentException("Resource must be of type OfficeWorker.", "resource");
            }
        }

        /// <summary>
        /// Tracks the OfficeWorker's ExecutionMode.
        /// </summary>
        public ExecutionMode ExecutionMode
        {
            get { return EnumUtil.Parse<ExecutionMode>(DBRunMode); }
            set { DBRunMode = value.ToString(); }
        }

        /// <summary>
        /// Checks the integrity of the data for this virtual resource.
        /// </summary>
        /// <returns>A list of strings, each of which provides information about a data integrity issue.</returns>
        public override IEnumerable<string> ValidateData()
        {
            if (!this.VirtualResourceMetadataSet.Any())
            {
                yield return string.Format("{0} contains no activities.", this.Name);
            }
            else if (!this.VirtualResourceMetadataSet.Where(n => n.Enabled).Any())
            {
                yield return string.Format("{0} contains no enabled activities.", this.Name);
            }
        }
    }
}
