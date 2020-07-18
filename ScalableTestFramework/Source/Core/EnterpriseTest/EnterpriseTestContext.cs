using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Entity framework context for connecting to the Enterprise Test database.
    /// </summary>
    public sealed class EnterpriseTestContext : DbContext
    {
        static EnterpriseTestContext()
        {
            // Disable code-first migration and database creation
            Database.SetInitializer<EnterpriseTestContext>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestContext" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="EnterpriseTestConnectionString" />.</param>
        public EnterpriseTestContext(EnterpriseTestConnectionString connectionString)
            : base(connectionString.ToString())
        {
        }

        /// <summary>
        /// Gets a collection of <see cref="ActiveDirectoryGroup" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<ActiveDirectoryGroup> ActiveDirectoryGroups => Set<ActiveDirectoryGroup>();

        /// <summary>
        /// Gets a collection of <see cref="AssociatedProduct" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<AssociatedProduct> AssociatedProducts => Set<AssociatedProduct>();

        /// <summary>
        /// Gets a collection of <see cref="CategoryValue" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<CategoryValue> CategoryValues => Set<CategoryValue>();

        /// <summary>
        /// Gets a collection of <see cref="ConfigurationTreeFolder" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<ConfigurationTreeFolder> ConfigurationTreeFolders => Set<ConfigurationTreeFolder>();

        /// <summary>
        /// Gets a collection of <see cref="EnterpriseScenario" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<EnterpriseScenario> EnterpriseScenarios => Set<EnterpriseScenario>();

        /// <summary>
        /// Gets a collection of <see cref="MetadataType" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<MetadataType> MetadataTypes => Set<MetadataType>();

        /// <summary>
        /// Gets a collection of <see cref="ResourceType" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<ResourceType> ResourceTypes => Set<ResourceType>();

        /// <summary>
        /// Gets a collection of <see cref="SoftwareInstaller" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<SoftwareInstaller> SoftwareInstallers => Set<SoftwareInstaller>();

        /// <summary>
        /// Gets a collection of <see cref="SoftwareInstallerPackage" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<SoftwareInstallerPackage> SoftwareInstallerPackages => Set<SoftwareInstallerPackage>();

        /// <summary>
        /// Gets a collection of <see cref="SystemSetting" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();

        /// <summary>
        /// Gets a collection of <see cref="User" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<User> Users => Set<User>();

        /// <summary>
        /// Gets a collection of <see cref="UserGroup" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<UserGroup> UserGroups => Set<UserGroup>();

        /// <summary>
        /// Gets a collection of <see cref="VirtualResource" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<VirtualResource> VirtualResources => Set<VirtualResource>();

        /// <summary>
        /// Gets a collection of <see cref="VirtualResourceMetadata" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<VirtualResourceMetadata> VirtualResourceMetadataSet => Set<VirtualResourceMetadata>();

        /// <summary>
        /// Gets a collection of <see cref="VirtualResourceMetadataRetrySetting" /> objects from the Enterprise Test database.
        /// </summary>
        public DbSet<VirtualResourceMetadataRetrySetting> VirtualResourceMetadataRetrySettings => Set<VirtualResourceMetadataRetrySetting>();

        /// <summary>
        /// Configures the entity framework model for the context.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Do not use EF pluralizing conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Define Table-Per-Type inheritance
            modelBuilder.Entity<AdminWorker>().ToTable("AdminWorker");
            modelBuilder.Entity<CitrixWorker>().ToTable("CitrixWorker");
            modelBuilder.Entity<EventLogCollector>().ToTable("EventLogCollector");
            modelBuilder.Entity<LoadTester>().ToTable("LoadTester");
            modelBuilder.Entity<OfficeWorker>().ToTable("OfficeWorker");
            modelBuilder.Entity<PerfMonCollector>().ToTable("PerfMonCollector");
            modelBuilder.Entity<SolutionTester>().ToTable("SolutionTester");

            // Define one-to-optional keys for VirtualResourceMetadata
            modelBuilder.Entity<VirtualResourceMetadata>().HasOptional(n => n.AssetUsage).WithRequired();
            modelBuilder.Entity<VirtualResourceMetadata>().HasOptional(n => n.DocumentUsage).WithRequired();
            modelBuilder.Entity<VirtualResourceMetadata>().HasOptional(n => n.PrintQueueUsage).WithRequired();
            modelBuilder.Entity<VirtualResourceMetadata>().HasOptional(n => n.ServerUsage).WithRequired();
            modelBuilder.Entity<VirtualResourceMetadataAssetUsage>().HasKey(n => n.VirtualResourceMetadataId);
            modelBuilder.Entity<VirtualResourceMetadataDocumentUsage>().HasKey(n => n.VirtualResourceMetadataId);
            modelBuilder.Entity<VirtualResourceMetadataPrintQueueUsage>().HasKey(n => n.VirtualResourceMetadataId);
            modelBuilder.Entity<VirtualResourceMetadataServerUsage>().HasKey(n => n.VirtualResourceMetadataId);

            // Declare primary keys for cases where EF will not infer them
            modelBuilder.Entity<ActiveDirectoryGroup>().HasKey(n => n.Name);
            modelBuilder.Entity<AssociatedProductVersion>().HasKey(n => new { n.AssociatedProductId, n.EnterpriseScenarioId });
            modelBuilder.Entity<EnterpriseScenarioSession>().HasKey(n => new { n.EnterpriseScenarioId, n.Name });
            modelBuilder.Entity<MetadataType>().HasKey(n => n.Name);
            modelBuilder.Entity<ResourceType>().HasKey(n => n.Name);
            modelBuilder.Entity<ResourceTypeFrameworkClientPlatformAssociation>().HasKey(n => new { n.FrameworkClientPlatformId, n.ResourceTypeName });
            modelBuilder.Entity<SoftwareInstallerPackageItem>().HasKey(n => new { n.SoftwareInstallerPackageId, n.SoftwareInstallerId });
            modelBuilder.Entity<SystemSetting>().HasKey(n => new { n.Type, n.SubType, n.Name });
            modelBuilder.Entity<User>().HasKey(n => n.UserName);
            modelBuilder.Entity<UserGroup>().HasKey(n => n.Name);
            modelBuilder.Entity<UserGroupFrameworkClientAssociation>().HasKey(n => new { n.UserGroupName, n.FrameworkClientHostName });

            // Declare one-to-many and many-to-many foreign key associations
            modelBuilder.Entity<AssociatedProduct>().HasMany(n => n.MetadataTypes).WithMany().Map(n => n.ToTable("AssociatedProductMetadataTypeAssoc").MapLeftKey("AssociatedProductId").MapRightKey("MetadataTypeName"));
            modelBuilder.Entity<CategoryValue>().HasMany(n => n.Parents).WithMany(n => n.Children).Map(n => n.ToTable("CategoryValueParent").MapLeftKey("CategoryValueId").MapRightKey("ParentCategoryValueId"));
            modelBuilder.Entity<EnterpriseScenario>().HasMany(n => n.UserGroups).WithMany().Map(n => n.ToTable("EnterpriseScenarioUserGroupAssoc").MapLeftKey("EnterpriseScenarioId").MapRightKey("UserGroupName"));
            modelBuilder.Entity<ResourceType>().HasMany(n => n.MetadataTypes).WithMany().Map(n => n.ToTable("ResourceTypeMetadataTypeAssoc").MapLeftKey("ResourceTypeName").MapRightKey("MetadataTypeName"));
            modelBuilder.Entity<SoftwareInstallerPackage>().HasMany(n => n.MetadataTypes).WithMany().Map(n => n.ToTable("SoftwareInstallerPackageMetadataTypeAssoc").MapLeftKey("SoftwareInstallerPackageId").MapRightKey("MetadataTypeName"));
            modelBuilder.Entity<SoftwareInstallerPackage>().HasMany(n => n.ResourceTypes).WithMany().Map(n => n.ToTable("SoftwareInstallerPackageResourceTypeAssoc").MapLeftKey("SoftwareInstallerPackageId").MapRightKey("ResourceTypeName"));
            modelBuilder.Entity<UserGroup>().HasMany(n => n.Users).WithMany().Map(n => n.ToTable("UserGroupAssoc").MapLeftKey("UserGroupName").MapRightKey("UserName"));
        }
    }
}
