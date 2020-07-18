using System;
using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Dispatcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.STF.Dispatcher.Library
{
    [TestClass]
    public class VirtualResourcePackerTest
    {
        [TestMethod]
        public void TestPlatformCount()
        {
            Collection<VirtualResource> resources = new Collection<VirtualResource>();
            resources.Add("XP", 15, 6);
            resources.Add("W7", 15, 13);

            VirtualResourcePacker resourcePacker = new VirtualResourcePacker(resources);
            Assert.AreEqual(2, resourcePacker.Platforms.Count);
            Assert.IsTrue(resourcePacker.Platforms.Contains("XP"));
            Assert.IsTrue(resourcePacker.Platforms.Contains("W7"));
        }

        [TestMethod]
        public void TestResourceCount()
        {
            Collection<VirtualResource> resources = new Collection<VirtualResource>();
            resources.Add("XP", 15, 17);
            resources.Add("W7", 15, 13);

            VirtualResourcePacker resourcePacker = new VirtualResourcePacker(resources);
            Assert.AreEqual(30, resourcePacker.TotalResourceCount);
        }

        [TestMethod]
        public void TestSimpleVirtualMachineCount()
        {
            // 30 total resources at 15 per VM should take 2 VMs
            Collection<VirtualResource> resources = new Collection<VirtualResource>();
            resources.Add("XP", 15, 1);
            resources.Add("XP", 15, 1);
            resources.Add("XP", 15, 28);

            VirtualResourcePacker resourcePacker = new VirtualResourcePacker(resources);
            Collection<VirtualResourcePackedSet> packedSets = resourcePacker.PackedSets;

            Assert.AreEqual(2, packedSets.Count);
            Assert.AreEqual(15, packedSets[0].Count);
            Assert.AreEqual(15, packedSets[1].Count);
        }

        [TestMethod]
        public void TestComplexVirtualMachineCount()
        {
            Collection<VirtualResource> resources = new Collection<VirtualResource>();
            resources.Add("W7", 5, 7);
            resources.Add("XP", 10, 8);
            resources.Add("W7", 15, 7);
            resources.Add("XP", 1, 8);

            VirtualResourcePacker resourcePacker = new VirtualResourcePacker(resources);
            Assert.AreEqual(3, resourcePacker.VirtualMachineQuantities["W7"]);
            Assert.AreEqual(9, resourcePacker.VirtualMachineQuantities["XP"]);
        }

        [TestMethod]
        public void TestSimplePack()
        {
            // If the each resource type is packed independently, this would take 4 VMs
            // If packed correctly, it should only take 3
            Collection<VirtualResource> resources = new Collection<VirtualResource>();
            resources.Add("XP", 3, 1);
            resources.Add("XP", 5, 2);
            resources.Add("XP", 10, 11);

            VirtualResourcePacker resourcePacker = new VirtualResourcePacker(resources);
            Collection<VirtualResourcePackedSet> packedSets = resourcePacker.PackedSets;

            ValidateContents(resources, packedSets);
            ValidatePacking(packedSets);
            Assert.AreEqual(3, packedSets.Count);
        }

        [TestMethod]
        public void TestComplexPack()
        {
            Collection<VirtualResource> resources = new Collection<VirtualResource>();
            resources.Add("XP", 5, 6);
            resources.Add("XP", 10, 13);
            resources.Add("XP", 15, 1);
            resources.Add("XP", 1, 4);
            resources.Add("XP", 15, 30);

            VirtualResourcePacker resourcePacker = new VirtualResourcePacker(resources);
            Collection<VirtualResourcePackedSet> packedSets = resourcePacker.PackedSets;

            ValidateContents(resources, packedSets);
            ValidatePacking(packedSets);
            Assert.AreEqual(9, packedSets.Count);
            Assert.AreEqual(10, packedSets[6].Count);
        }

        [TestMethod]
        public void TestPackOrder()
        {
            // This order was problematic for a past algorithm,
            // but swapping the 3rd and 4th resources made it work.
            Collection<VirtualResource> resources = new Collection<VirtualResource>();
            resources.Add("XP", 10, 6);
            resources.Add("XP", 10, 8);
            resources.Add("XP", 15, 2);
            resources.Add("XP", 15, 8);

            VirtualResourcePacker resourcePacker = new VirtualResourcePacker(resources);
            Collection<VirtualResourcePackedSet> packedSets = resourcePacker.PackedSets;

            ValidateContents(resources, packedSets);
            ValidatePacking(packedSets);
            Assert.AreEqual(3, packedSets.Count);
        }

        [TestMethod]
        public void TestCascade()
        {
            // The first resource of each type will "cascade" into the gap left by
            // the one before it, leaving one gap open for the next.
            // The last resource in each set should have ResourcePerVM 1 higher than the rest.
            Collection<VirtualResource> resources = new Collection<VirtualResource>();
            resources.Add("XP", 2, 1);
            resources.Add("XP", 3, 3);
            resources.Add("XP", 4, 4);
            resources.Add("XP", 5, 5);
            resources.Add("XP", 6, 6);
            resources.Add("XP", 7, 7);
            resources.Add("XP", 8, 1);

            VirtualResourcePacker resourcePacker = new VirtualResourcePacker(resources);
            Collection<VirtualResourcePackedSet> packedSets = resourcePacker.PackedSets;

            ValidateContents(resources, packedSets);
            ValidatePacking(packedSets);
            Assert.AreEqual(6, packedSets.Count);
            foreach (VirtualResourcePackedSet collection in packedSets)
            {
                Assert.AreEqual(collection.First().ResourcesPerVM, collection.Last().ResourcesPerVM - 1);
            }
        }

        [TestMethod]
        public void TestMultiPlatformPack()
        {
            // If all of the platforms are the same, this will take only 6 VMs
            // If platforms are taken into account, this should take 7
            Collection<VirtualResource> resources = new Collection<VirtualResource>();
            resources.Add("W7", 15, 8);
            resources.Add("XP", 10, 12);
            resources.Add("W7", 10, 6);
            resources.Add("W7", 10, 8);
            resources.Add("XP", 2, 3);

            VirtualResourcePacker resourcePacker = new VirtualResourcePacker(resources);
            Collection<VirtualResourcePackedSet> packedSets = resourcePacker.PackedSets;

            ValidateContents(resources, packedSets);
            ValidatePacking(packedSets);
            Assert.AreEqual(7, packedSets.Count);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCollectionOverflow()
        {
            VirtualResourcePackedSet resources = new VirtualResourcePackedSet("XP", 1);
            Assert.IsFalse(resources.IsFullyPacked);

            resources.Add("XP", 15, 1);
            Assert.IsTrue(resources.IsFullyPacked);

            // This line should throw an InvalidOperationException
            resources.Add("XP", 15, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCollectionPlatformMismatch()
        {
            // This line should throw an InvalidOperationException
            new VirtualResourcePackedSet("XP", 1).Add("W7", 15, 1);
        }

        private static void ValidateContents(Collection<VirtualResource> resources, Collection<VirtualResourcePackedSet> packedSets)
        {
            // Make sure the resources in the packed sets match up with the resources provided to the collection
            foreach (VirtualResource resource in resources)
            {
                Assert.AreEqual(resource.InstanceCount, packedSets.Sum(n => n.Count(m => m.Name == resource.Name)));
            }
        }

        private static void ValidatePacking(Collection<VirtualResourcePackedSet> packedSets)
        {
            // No resource should be packed with more resources than it wants to be
            foreach (VirtualResourcePackedSet packedSet in packedSets)
            {
                foreach (VirtualResource resource in packedSet)
                {
                    Assert.IsFalse(packedSet.Count > (int)resource.ResourcesPerVM);
                }
            }
        }
    }

    internal static partial class Extension
    {
        private static Guid scenarioId = Guid.NewGuid();
        private static Guid folderId = Guid.NewGuid();

        public static void Add(this Collection<VirtualResource> resources, string platform, int resourcesPerVM, int instanceCount)
        {
            OfficeWorker ow = new OfficeWorker(Guid.NewGuid(), scenarioId, folderId);
            ow.Name = string.Format("OfficeWorker #{0}", resources.Count + 1);
            ow.Platform = platform;
            ow.ResourcesPerVM = resourcesPerVM;
            ow.InstanceCount = instanceCount;
            resources.Add(ow);
        }
    }
}
