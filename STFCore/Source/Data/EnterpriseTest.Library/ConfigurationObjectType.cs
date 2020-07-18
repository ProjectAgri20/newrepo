
namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// Object types used to configure test scenarios.
    /// </summary>
    public enum ConfigurationObjectType
    {
        /// <summary>
        /// Unknown type
        /// </summary>
        Unknown,

        /// <summary>
        /// Enterprise Scenario
        /// </summary>
        EnterpriseScenario,

        /// <summary>
        /// Virtual Resource
        /// </summary>
        VirtualResource,

        /// <summary>
        /// Virtual Resource Metadata
        /// </summary>
        ResourceMetadata,

        /// <summary>
        /// Folder containing scenarios
        /// </summary>
        ScenarioFolder,

        /// <summary>
        /// Folder containing resources
        /// </summary>
        ResourceFolder,

        /// <summary>
        /// Folder containing metadata
        /// </summary>
        MetadataFolder
    }

    /// <summary>
    /// Helper class that holds extension methods for <see cref="ConfigurationObjectType"/>.
    /// </summary>
    public static class ConfigurationObjectTypeHelper
    {
        /// <summary>
        /// Determines whether the specified map type is a folder type.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <returns>
        ///   <c>true</c> if the specified map type is a folder type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFolder(this ConfigurationObjectType mapType)
        {
            return (mapType == ConfigurationObjectType.ScenarioFolder
                 || mapType == ConfigurationObjectType.ResourceFolder
                 || mapType == ConfigurationObjectType.MetadataFolder);
        }

        /// <summary>
        /// Determines whether this instance can contain the specified map type.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <param name="childType">Type of the child.</param>
        /// <returns>
        ///   <c>true</c> if this instance can contain the specified map type; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanContain(this ConfigurationObjectType mapType, ConfigurationObjectType childType)
        {
            switch (mapType)
            {
                case ConfigurationObjectType.ScenarioFolder:
                    return childType == ConfigurationObjectType.ScenarioFolder
                        || childType == ConfigurationObjectType.EnterpriseScenario;

                case ConfigurationObjectType.EnterpriseScenario:
                case ConfigurationObjectType.ResourceFolder:
                    return childType == ConfigurationObjectType.ResourceFolder
                        || childType == ConfigurationObjectType.VirtualResource;

                case ConfigurationObjectType.VirtualResource:
                case ConfigurationObjectType.MetadataFolder:
                    return childType == ConfigurationObjectType.MetadataFolder
                        || childType == ConfigurationObjectType.ResourceMetadata;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the specified map type can be enabled/disabled.
        /// </summary>
        /// <param name="mapType">Type of the map.</param>
        /// <returns>
        ///   <c>true</c> if the specified map type can be enabled/disabled; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanEnableDisable(this ConfigurationObjectType mapType)
        {
            switch (mapType)
            {
                case ConfigurationObjectType.VirtualResource:
                case ConfigurationObjectType.ResourceMetadata:
                    return true;

                default:
                    return false;
            }
        }
    }
}
