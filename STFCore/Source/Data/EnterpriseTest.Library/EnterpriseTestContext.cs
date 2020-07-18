using System;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects.DataClasses;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Settings;
using System.Linq;

namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// Entity framework context for connecting to the Enterprise Test database.
    /// </summary>
    public class EnterpriseTestContext : Model.EnterpriseTestEntities
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestContext"/> class.
        /// </summary>
        public EnterpriseTestContext()
            : base(EntityConnectionString())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestContext"/> class.
        /// </summary>
        /// <param name="database">The hostname/IP of the Enterprise Test database.</param>
        public EnterpriseTestContext(string database)
            : base(EntityConnectionString(database))
        {
        }

        /// <summary>
        /// Builds an entity connection string for the Enterprise Test database
        /// using the location defined in System Settings.
        /// </summary>
        /// <returns></returns>
        public static string EntityConnectionString()
        {
            return EntityConnectionString(GlobalSettings.Items[Setting.EnterpriseTestDatabase]);
        }

        /// <summary>
        /// Builds an entity connection string for the Enterprise Test database.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <returns></returns>
        public static string EntityConnectionString(string database)
        {
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder()
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = EnterpriseTestSqlConnection.BuildConnectionString(database),
                Metadata = @"res://*/EnterpriseTestModel.csdl|res://*/EnterpriseTestModel.ssdl|res://*/EnterpriseTestModel.msl"
            };

            return entityBuilder.ToString();
        }

        /// <summary>
        /// Finalizes changes to objects to ensure that they have the correct state.
        /// </summary>
        internal void FinalizeChanges()
        {
            foreach (EntityObject entity in this.GetObjectsInState(EntityState.Added | EntityState.Modified).Where(x => x != null))
            {
                // Check modified resources and metadata to see if they have been orphaned
                // If so, change their entity state to Deleted
                VirtualResource resource = entity as VirtualResource;
                if (resource != null && resource.EnterpriseScenarioId == Guid.Empty)
                {
                    DeleteObject(resource);
                    continue;
                }

                VirtualResourceMetadata metadata = entity as VirtualResourceMetadata;
                if (metadata != null && metadata.VirtualResourceId == Guid.Empty)
                {
                    DeleteObject(metadata);
                    continue;
                }

                VirtualResourceMetadataRetrySetting setting = entity as VirtualResourceMetadataRetrySetting;
                if (setting != null && setting.VirtualResourceMetadataId == Guid.Empty)
                {
                    DeleteObject(setting);
                    continue;
                }

                // Check object to see if there are actually any changed properties
                // If nothing has been modified, the state will be changed back to unchanged
                this.CheckIfModified(entity);
            }
        }
    }
}
