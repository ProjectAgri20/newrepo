using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Defines a set of <see cref="ScenarioPlatformUsage"/>s.
    /// </summary>
    public class ScenarioPlatformUsageSet : Collection<ScenarioPlatformUsage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioPlatformUsageSet"/> class.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public ScenarioPlatformUsageSet(EnterpriseScenario scenario)
        {
            Scenario = scenario;
        }

        /// <summary>
        /// Gets the scenario.
        /// </summary>
        /// <value>The scenario.</value>
        public EnterpriseScenario Scenario { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [required platforms available].
        /// </summary>
        /// <value><c>true</c> if [required platforms available]; otherwise, <c>false</c>.</value>
        public bool PlatformsAvailable
        {
            get
            {
                foreach (ScenarioPlatformUsage item in this)
                {
                    if (item.RequiredCount > item.AuthorizedCount || (item.RequiredCount == 0 && item.AuthorizedCount == 0))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Loads based on the specified user and role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        public void Load(UserCredential user)
        {
            SessionResourceQuantity quantity = GetQuantities(Scenario);

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (string platformId in quantity.MachineQuantity.Keys)
                {
                    FrameworkClientPlatform platform = context.FrameworkClientPlatforms.FirstOrDefault(x => x.FrameworkClientPlatformId.Equals(platformId));
                    int count = VirtualMachine.GetUsageCountByPlatform(platformId, user);

                    var usage = new ScenarioPlatformUsage()
                    {
                        PlatformId = platformId,
                        Description = platform.Name,
                        RequiredCount = quantity.MachineQuantity[platformId],
                        AuthorizedCount = count
                    };

                    Add(usage);
                }
            }
        }

        /// <summary>
        /// Loads based on the specified user and Hold Id.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        public void Load(UserCredential user, string holdId)
        {
            SessionResourceQuantity quantity = GetQuantities(Scenario);

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (string platformId in quantity.MachineQuantity.Keys)
                {
                    FrameworkClientPlatform platform = context.FrameworkClientPlatforms.FirstOrDefault(x => x.FrameworkClientPlatformId.Equals(platformId));
                    int count = VirtualMachine.GetUsageCountByHold(platformId, holdId, user);

                    var usage = new ScenarioPlatformUsage()
                    {
                        PlatformId = platformId,
                        Description = platform.Name,
                        RequiredCount = quantity.MachineQuantity[platformId],
                        AuthorizedCount = count
                    };

                    Add(usage);
                }
            }
        }

        /// <summary>
        /// Loads based on the specified user and Platform.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The user role (User, Admin, etc.).</param>
        /// <param name="platform">The virtual machine platform.</param>
        public void Load(UserCredential user, FrameworkClientPlatform platform)
        {
            SessionResourceQuantity quantity = GetQuantities(Scenario);

            int requiredCount = 0;
            if (quantity.MachineQuantity.ContainsKey(platform.FrameworkClientPlatformId))
            {
                requiredCount = quantity.MachineQuantity[platform.FrameworkClientPlatformId];
            }

            int count = VirtualMachine.GetUsageCountByPlatform(platform.FrameworkClientPlatformId, user);

            var usage = new ScenarioPlatformUsage()
            {
                PlatformId = platform.FrameworkClientPlatformId,
                Description = platform.Name,
                RequiredCount = requiredCount,
                AuthorizedCount = count
            };

            Add(usage);
        }

        private SessionResourceQuantity GetQuantities(EnterpriseScenario scenario)
        {
            // Force a load of VirtualResourceMetadata for this scenario.  Otherwise, when the VirtualResourcePacker
            // is doing its job the call to Clone() in the PackedSets property of packer will not deep copy the
            // Metadata.  This seems to be an issue with how the serialization is working when the entity is in 
            // a lazy loading mode.  This code is just forcing the VirtualResourceMetadata to be loaded.
            scenario.VirtualResources.SelectMany(x => x.VirtualResourceMetadataSet).Count();

            // Determine the quantities for this scenario
            return new SessionResourceQuantity(scenario.VirtualResources.Where(x => x.Enabled == true));
        }
    }
}
