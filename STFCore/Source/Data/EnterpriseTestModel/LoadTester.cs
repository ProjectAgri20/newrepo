using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Xml;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    [ObjectFactory(VirtualResourceType.LoadTester)]
    public partial class LoadTester
    {
        private class ExpandedMetadata
        {
            public VirtualResourceMetadata Metadata { get; set; }
            public int ThreadCount { get; set; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadTester"/> class.
        /// </summary>
        public LoadTester()
            : this("LoadTester")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadTester"/> class.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        public LoadTester(string resourceType)
            : base(resourceType)
        {
            ThreadsPerVM = 100;
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

        public override void CopyResourceProperties(VirtualResource resource)
        {
            base.CopyResourceProperties(resource);
            LoadTester loadTester = resource as LoadTester;
            if (loadTester != null)
            {
                this.ThreadsPerVM = loadTester.ThreadsPerVM;
            }
            else
            {
                throw new ArgumentException("Resource must be of type LoadTester.", "resource");
            }            
        }

        protected override void LoadChildDetail(ResourceDetailBase detail)
        {
            var resourceDetail = detail as LoadTesterDetail;
            ThreadsPerVM = resourceDetail.ThreadsPerVM;
            Enabled = resourceDetail.Enabled;  
        }

        protected override string GetExecutionPlan(ResourceMetadataDetail detail)
        {
            return LegacySerializer.SerializeDataContract((LoadTesterExecutionPlan)detail.Plan).ToString();

        }

        public override IEnumerable<VirtualResource> ExpandedDefinitions
        {
            get
            {
                int resourceIndex = 1;

                TraceFactory.Logger.Debug("Expanding resources");

                // Load a collection of metadata Ids and associated thread counts into a 
                // working set that will be used to expand out the number of resources
                // needed to meet the thread per VM value.
                Collection<OriginalResourceMetadata> originalMetadataSet = new Collection<OriginalResourceMetadata>();
                foreach (var item in VirtualResourceMetadataSet.Where(e => e.Enabled == true))
                {
                    var plan = LegacySerializer.DeserializeDataContract<LoadTesterExecutionPlan>(item.ExecutionPlan);
                    originalMetadataSet.Add(new OriginalResourceMetadata(item.VirtualResourceMetadataId, plan));
                }

                int index = 0;
                Collection<ExpandedResourceMetadata> expandedMetadataSet = null;

                // Iterate over the working data, and for each entry decrement the thread count
                // as that count is added to an expanded set.   The expanded set will represent
                // all the metadata and associated thread counts that can be supported for one
                // Load Tester.  When the expanded set is filled then a new Virtual Resource will
                // be created and returned to the client.
                while (originalMetadataSet.Count > 0)
                {
                    if (expandedMetadataSet == null)
                    {
                        expandedMetadataSet = new Collection<ExpandedResourceMetadata>();
                    }

                    // Using an circular index, get the current working data value
                    var currentMetadata = originalMetadataSet[index];

                    // Look for an entry in the expanded resources for this metadata Id.  If it's not there
                    // then create it, add it to the collection and increment its new thread count.
                    var expandedMetadata = expandedMetadataSet.FirstOrDefault(x => x.Id == currentMetadata.Id);
                    if (expandedMetadata == null)
                    {
                        expandedMetadata = new ExpandedResourceMetadata(currentMetadata.Id);
                        expandedMetadataSet.Add(expandedMetadata);
                    }
                    expandedMetadata.ThreadCount++;

                    if (currentMetadata.Mode == RampUpMode.RateBased)
                    {
                        // Update information in the expanded resources on the ramp up delay.  Since the expanded
                        // resources may cause threads to be spread across multiple VMs, it's still important to
                        // ensure that the aggregate ramp up rate is still preserved.  This increments the thread
                        // count at the current rampup interval.
                        var rampUp = expandedMetadata.RampUpSettings.FirstOrDefault(x => x.Delay == currentMetadata.WorkingRampUpInterval);
                        if (rampUp == null)
                        {
                            // This ramp up setting doesn't exist, so create a new one, then increment its thread count
                            rampUp = new RampUpSetting(currentMetadata.WorkingRampUpInterval);
                            expandedMetadata.RampUpSettings.Add(rampUp);
                        }
                        rampUp.ThreadCount++;
                        currentMetadata.UpdateRampUp();
                    }

                    // Decrement the thread count from the current metadata, then check to see if it is empty.
                    // If it is then remove it from the collection.  It also checks to see if the thread count
                    // at the current ramp up is depleted, and if it is it will increment the interval and refill
                    // the thread count.
                    currentMetadata.TotalThreads--;
                    if (currentMetadata.TotalThreads == 0)
                    {
                        originalMetadataSet.Remove(currentMetadata);
                        if (originalMetadataSet.Count == 0)
                        {
                            break;
                        }

                        // This is here to ensure the circular index rolls back around to zero when its value
                        // is equal to the new count of original resources.  This is a corner case.
                        if (index == originalMetadataSet.Count)
                        {
                            index--;
                        }
                    }

                    // Increment the circular index and mod it by the working data set count
                    // This will get the next index value to use.
                    index = (index + 1) % originalMetadataSet.Count;

                    if (expandedMetadataSet.Sum(x => x.ThreadCount) == ThreadsPerVM)
                    {
                        // If the expanded set now contains a count equal to the maximum number of threads per VM,
                        // then create a new LoadTester resource with the metadata and thread counts defined in
                        // the expaned set, and then return the new resource to the client.
                        yield return CreateExpandedResource(expandedMetadataSet, resourceIndex++);

                        expandedMetadataSet = null;
                        index = 0;
                    }
                }

                // We are now out of the loop, but if there are threads in the last expanded resource
                // collection, then ensure one last LoadTester resource is created and returned.
                if (expandedMetadataSet != null && expandedMetadataSet.Sum(x => x.ThreadCount) > 0)
                {
                    yield return CreateExpandedResource(expandedMetadataSet, resourceIndex);
                }
            }
        }

        private VirtualResource CreateExpandedResource(Collection<ExpandedResourceMetadata> expandedData, int index)
        {
            // First clone this Load Tester resource, then clear it's metadata set.
            VirtualResource resource = this.Clone<LoadTester>();
            resource.VirtualResourceMetadataSet.Clear();

            resource.Name = "{0} [{1}]".FormatWith(resource.Name, index);

            // For each entry in the expanded set, get the metadata item from "this" by the id
            // in the expanded set item, clone it, then update the thread count to the value
            // in the expanded set item, then add the new metadata item to the resource.  Do 
            // this for all items in the expanded data.  Then return the resource.
            foreach (var item in expandedData)
            {
                var metadata = VirtualResourceMetadataSet.First(x => x.VirtualResourceMetadataId == item.Id).Clone();

                // Update the execution plan with the correct number of threads and update it with
                // the property thread ramp up information.
                var plan = LegacySerializer.DeserializeDataContract<LoadTesterExecutionPlan>(metadata.ExecutionPlan);
                plan.ThreadCount = item.ThreadCount;

                plan.RampUpSettings = new Collection<RampUpSetting>();
                foreach (var setting in item.RampUpSettings)
                {
                    plan.RampUpSettings.Add(setting);
                }
                metadata.ExecutionPlan = LegacySerializer.SerializeDataContract(plan).ToString();
                resource.VirtualResourceMetadataSet.Add(metadata);
            }

            return resource;
        }

        // Class used to support the expansion of the load tester resources
        private class OriginalResourceMetadata
        {
            private int _rampUpThreadsConfig = 0;
            private TimeSpan _rampUpIntervalConfig = TimeSpan.Zero;
            private TimeSpan _workingRampUpInterval = TimeSpan.Zero;
            private int _workingRampUpThreads = 0;

            public Guid Id { get; private set; }
            public int TotalThreads { get; set; }
            public RampUpMode Mode { get; private set; }

            public TimeSpan WorkingRampUpInterval
            {
                get { return _workingRampUpInterval; }
            }

            public void UpdateRampUp()
            {
                _workingRampUpThreads--;

                if (_workingRampUpThreads == 0)
                {
                    // If the thread count for ramp up threads is 0, then increment the time
                    // interval and refill the thread count.
                    _workingRampUpThreads = _rampUpThreadsConfig;
                    _workingRampUpInterval = WorkingRampUpInterval.Add(_rampUpIntervalConfig);
                }
            }

            public OriginalResourceMetadata(Guid id, LoadTesterExecutionPlan plan)
            {
                Id = id;
                _rampUpThreadsConfig = plan.RampUpPacingThreads;
                _rampUpIntervalConfig = plan.RampUpPacingDelay;
                _workingRampUpThreads = plan.RampUpPacingThreads;

                TotalThreads = plan.ThreadCount;
                Mode = plan.RampUpMode;

                if (plan.DelayOneInterval)
                {
                    _workingRampUpInterval = _workingRampUpInterval.Add(_rampUpIntervalConfig);
                }
            }
        }

        private class ExpandedResourceMetadata
        {
            public Guid Id { get; private set; }
            public int ThreadCount { get; set; }
            public Collection<RampUpSetting> RampUpSettings { get; private set; }

            public ExpandedResourceMetadata(Guid id)
            {
                Id = id;
                RampUpSettings = new Collection<RampUpSetting>();
                ThreadCount = 0;
            }
        }
    }
}

