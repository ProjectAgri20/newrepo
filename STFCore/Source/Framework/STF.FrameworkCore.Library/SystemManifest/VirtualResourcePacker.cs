using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    public class VirtualResourcePacker
    {
        private readonly Collection<VirtualResourcePackedSet> _packedSets = new Collection<VirtualResourcePackedSet>();
        private readonly IEnumerable<VirtualResource> _resources = null;

        /// <summary>
        /// Gets the platforms.
        /// </summary>
        public Collection<string> Platforms
        {
            get { return new Collection<string>(_packedSets.Select(n => n.Platform).Distinct().ToList()); }
        }

        /// <summary>
        /// Gets the total resource count.
        /// </summary>
        public int TotalResourceCount(VirtualResourceType resourceType)
        {
            return _packedSets.Where(x => x.ResourceType == resourceType).Sum(n => n.Count); 
        }

        /// <summary>
        /// Gets the virtual machine quantities.
        /// </summary>
        public Dictionary<string, int> VirtualMachineQuantities
        {
            get
            {
                Dictionary<string, int> quantities = new Dictionary<string, int>();
                foreach (string platform in Platforms)
                {
                    int count = _packedSets.Count(n => n.Platform.Equals(platform, StringComparison.OrdinalIgnoreCase));
                    quantities.Add(platform, count);
                }
                return quantities;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResourcePacker"/> class.
        /// </summary>
        /// <param name="resources">The resources.</param>
        public VirtualResourcePacker(IEnumerable<VirtualResource> resources)
        {
            _resources = resources;
        }

        public IEnumerable<VirtualResourcePackedSet> PackedSets
        {
            get
            {
                if (_packedSets.Count == 0 && _resources.Count() > 0)
                {
                    // Group all the resources by platform
                    var resourcesByPlatform = new Dictionary<string, Collection<VirtualResource>>();
                    foreach (VirtualResource resource in _resources)
                    {
                        if (!resourcesByPlatform.ContainsKey(resource.Platform))
                        {
                            resourcesByPlatform.Add(resource.Platform, new Collection<VirtualResource>());
                        }

                        // Clone all the resources so we can change values without affecting the originals
                        var clone = resource.CloneWithEagerLoad();
                        resourcesByPlatform[resource.Platform].Add(clone);
                    }

                    // For each platform, create the packed sets
                    foreach (string platform in resourcesByPlatform.Keys)
                    {
                        PackResources(platform, resourcesByPlatform[platform]);
                    }
                }

                foreach (var packedSet in _packedSets)
                {
                    yield return packedSet;
                }
            }
        }

        private void PackResources(string platform, Collection<VirtualResource> resourceCollection)
        {
            // Iterate over each collection of resources grouped by the resource type.
            foreach (var resources in resourceCollection.GroupBy(x => x.ResourceType))
            {
                // Some of the resources may not have ResourcesPerVM specified.
                // Fill those in with the default from the database so they all have good values for the sort.
                List<VirtualResource> resourcesWithNulls = resources.Where(n => n.ResourcesPerVM == null).ToList();
                if (resourcesWithNulls.Count() > 0)
                {
                    using (EnterpriseTestContext context = new EnterpriseTestContext())
                    {
                        int defaultResourcesPerVM = context.ResourceTypes.First(n => n.Name == resourcesWithNulls[0].ResourceType).MaxResourcesPerHost;
                        resourcesWithNulls.ForEach(n => n.ResourcesPerVM = defaultResourcesPerVM);
                    }
                }

                // Sort the resources by the maximum number of resources per VM
                // This is required for the greedy packing algorithm to work
                List<VirtualResource> orderedSet = resources.OrderBy(n => (int)n.ResourcesPerVM).ToList();

                // This algorithm will pack the resources into sets while mandating that no resource is packed
                // with more siblings than specified by its ResourcesPerVM property.
                // The algorithm starts with the most claustrophobic resources (lowest ResourcesPerVM) and
                // packs as many sets as necessary.  The last set packed this way may have some space left over,
                // so fill that with the next-most claustrophobic resource instance.  Continue this process
                // until all the VMs have been packed into sets.
                VirtualResourcePackedSet packingSet = null;
                foreach (VirtualResource resource in orderedSet.SelectMany(x => x.ExpandedDefinitions))
                {
                    // Each iteration through this block represents packing 1 instance of this resource
                    {
                        // If packingSet is null, this means we need to create a new packed set
                        // based on the resource that is currently being packed
                        if (packingSet == null)
                        {                            
                            packingSet = new VirtualResourcePackedSet(resource, platform);
                        }

                        // Add one instance of this resource to our current set
                        packingSet.Add(resource);

                        // Check to see if this set is fully packed
                        if (packingSet.IsFullyPacked)
                        {
                            // Save off this set into our listed of packed sets and null out our list
                            // Don't generate a new list yet - it might not be based on this resource
                            _packedSets.Add(packingSet);
                            packingSet = null;
                        }
                    }
                }

                // When we get to the end, we may have a partially packed set - if so, save it to our list
                if (packingSet != null && packingSet.Count > 0)
                {
                    _packedSets.Add(packingSet);
                }
            }
        }
    }    
}
