using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Virtualization;
using System.Linq;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Maintains a count of Virtual Machines by platform using <see cref="MachineQuantity"/> and a count
    /// of domain accounts required for the given SessionId
    /// </summary>
    public class SessionResourceQuantity
    {
        private readonly VMQuantityDictionary _vmQuantity = null;
        private readonly DomainAccountQuantityDictionary _domainAccountQuantity = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionResourceQuantity" /> class.
        /// </summary>
        public SessionResourceQuantity()
        {
            _vmQuantity = new VMQuantityDictionary();
            _domainAccountQuantity = new DomainAccountQuantityDictionary();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionResourceQuantity"/> class.
        /// </summary>
        /// <param name="packer">The packer.</param>
        public SessionResourceQuantity(VirtualResourcePacker packer)
            : this()
        {
            IncrementQuantities(packer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionResourceQuantity"/> class.
        /// </summary>
        /// <param name="resources">The Virtual Resources for a scenario.</param>
        public SessionResourceQuantity(IEnumerable<VirtualResource> resources)
            : this()
        {
            var packer = new VirtualResourcePacker(resources);
            IncrementQuantities(packer);
        }

        private void IncrementQuantities(VirtualResourcePacker packer)
        {
            // Get a list of distinct resources based on the resource Id.  Then for each distinct
            // resource increment the total user pool count by the instance count for the resource.
            var resourceList = packer.PackedSets
                .SelectMany(x => x)
                .DistinctBy(x => x.VirtualResourceId)
                .Where
                (
                    x => x.ResourceType.Equals("OfficeWorker") || 
                    x.ResourceType.Equals("CitrixWorker") ||
                    x.ResourceType.Equals("SolutionTester")
                );

            foreach (var resource in resourceList)
            {
                SolutionTester tester = resource as SolutionTester;
                if (tester != null && tester.AccountType != SolutionTesterCredentialType.AccountPool)
                {
                    continue;
                }

                IncrementDomainAccountQuantity(((OfficeWorker)resource).UserPool, resource.InstanceCount);
            }

            // For each packed set, which equates to the resources that will run on a given VM, increment
            // the VM count by platform used for each packed resource set.
            foreach (var packedResourceSet in packer.PackedSets)
            {
                IncrementVirtualMachineQuantity(packedResourceSet.Platform);
            }
        }

        /// <summary>
        /// Gets the VM quantity.
        /// </summary>
        public VMQuantityDictionary MachineQuantity
        {
            get { return _vmQuantity; }
        }

        /// <summary>
        /// Gets the domain account quantity.
        /// </summary>
        public DomainAccountQuantityDictionary DomainAccountQuantity
        {
            get { return _domainAccountQuantity; }
        }

        /// <summary>
        /// Increments the count of virtual machines for the given platform by 1
        /// </summary>
        /// <param name="platform">The platform.</param>
        public void IncrementVirtualMachineQuantity(string platform)
        {
            IncrementVirtualMachineQuantity(platform, 1);
        }

        /// <summary>
        /// Adds the count of virtual machines for the given platform.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <param name="count">The count.</param>
        public void IncrementVirtualMachineQuantity(string platform, int count)
        {
            if (!this.MachineQuantity.ContainsKey(platform))
            {
                this.MachineQuantity.Add(platform, 0);
            }
            this.MachineQuantity[platform] += count;
        }

        /// <summary>
        /// Adds domain accounts to the quantity.
        /// </summary>
        /// <param name="poolType">The account pool.</param>
        /// <param name="count">The count.</param>
        public void IncrementDomainAccountQuantity(string poolType, int count)
        {
            if (!this.DomainAccountQuantity.ContainsKey(poolType))
            {
                this.DomainAccountQuantity.Add(poolType, 0);
            }

            this.DomainAccountQuantity[poolType] += count;
        }
    }

    /// <summary>
    /// Helper class used to capture a domain account key and the the quantity.
    /// </summary>
    [Serializable]
    public class DomainAccountQuantityDictionary : SerializableDictionary<string, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainAccountQuantityDictionary"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected DomainAccountQuantityDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainAccountQuantityDictionary"/> class.
        /// </summary>
        public DomainAccountQuantityDictionary()
        {
        }

        /// <summary>
        /// Adds the maximum values per platform of the specified <see cref="DomainAccountQuantityDictionary"/> to the dictionary.
        /// </summary>
        /// <param name="quantity">The <see cref="DomainAccountQuantityDictionary"/> to add.</param>
        public void Add(DomainAccountQuantityDictionary quantity)
        {
            foreach (string key in quantity.Keys)
            {
                if (!Keys.Contains(key))
                {
                    this[key] = 0;
                }
                this[key] = Math.Max(this[key], quantity[key]);
            }
        }
    }
}
