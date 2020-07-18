using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// Entity framework context for connecting to the Asset Inventory database.
    /// </summary>
    public sealed class AssetInventoryContext : DbContext
    {
        /// <summary>
        /// Initializes the <see cref="AssetInventoryContext" /> class.
        /// </summary>
        static AssetInventoryContext()
        {
            // Disable code-first migration and database creation
            Database.SetInitializer<AssetInventoryContext>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInventoryContext" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="AssetInventoryConnectionString" />.</param>
        public AssetInventoryContext(AssetInventoryConnectionString connectionString)
            : base(connectionString.ToString())
        {
        }

        /// <summary>
        /// Gets a collection of <see cref="Asset" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<Asset> Assets => Set<Asset>();

        /// <summary>
        /// Gets a collection of <see cref="AssetPool" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<AssetPool> AssetPools => Set<AssetPool>();

        /// <summary>
        /// Gets a collection of <see cref="AssetReservation" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<AssetReservation> AssetReservations => Set<AssetReservation>();

        /// <summary>
        /// Gets a collection of <see cref="Badge" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<Badge> Badges => Set<Badge>();

        /// <summary>
        /// Gets a collection of <see cref="BadgeBox" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<BadgeBox> BadgeBoxes => Set<BadgeBox>();

        /// <summary>
        /// Gets a collection of <see cref="BashLogCollector" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<BashLogCollector> BashLogCollectors => Set<BashLogCollector>();

        /// <summary>
        /// Gets a collection of <see cref="CitrixPublishedApp" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<CitrixPublishedApp> CitrixPublishedApps => Set<CitrixPublishedApp>();

        /// <summary>
        /// Gets a collection of <see cref="DartBoard" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<DartBoard> DartBoards => Set<DartBoard>();

        /// <summary>
        /// Gets a collection of <see cref="DomainAccountPool" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<DomainAccountPool> DomainAccountPools => Set<DomainAccountPool>();

        /// <summary>
        /// Gets a collection of <see cref="DomainAccountReservation" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<DomainAccountReservation> DomainAccountReservations => Set<DomainAccountReservation>();

        /// <summary>
        /// Gets a collection of <see cref="ExternalCredential" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<ExternalCredential> ExternalCredentials => Set<ExternalCredential>();

        /// <summary>
        /// Gets a collection of <see cref="FrameworkClient" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<FrameworkClient> FrameworkClients => Set<FrameworkClient>();

        /// <summary>
        /// Gets a collection of <see cref="FrameworkClientPlatform" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<FrameworkClientPlatform> FrameworkClientPlatforms => Set<FrameworkClientPlatform>();

        /// <summary>
        /// Gets a collection of <see cref="FrameworkServer" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<FrameworkServer> FrameworkServers => Set<FrameworkServer>();

        /// <summary>
        /// Gets a collection of <see cref="FrameworkServerType" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<FrameworkServerType> FrameworkServerTypes => Set<FrameworkServerType>();

        /// <summary>
        /// Gets a collection of <see cref="License" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<License> Licenses => Set<License>();

        /// <summary>
        /// Gets a collection of <see cref="MonitorConfig" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<MonitorConfig> MonitorConfigs => Set<MonitorConfig>();

        /// <summary>
        /// Gets a collection of <see cref="PrintDriver" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<PrintDriver> PrintDrivers => Set<PrintDriver>();

        /// <summary>
        /// Gets a collection of <see cref="PrintDriverConfig" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<PrintDriverConfig> PrintDriverConfigs => Set<PrintDriverConfig>();

        /// <summary>
        /// Gets a collection of <see cref="PrintDriverPackage" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<PrintDriverPackage> PrintDriverPackages => Set<PrintDriverPackage>();

        /// <summary>
        /// Gets a collection of <see cref="PrintDriverProduct" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<PrintDriverProduct> PrintDriverProducts => Set<PrintDriverProduct>();

        /// <summary>
        /// Gets a collection of <see cref="PrintDriverVersion" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<PrintDriverVersion> PrintDriverVersions => Set<PrintDriverVersion>();

        /// <summary>
        /// Gets a collection of <see cref="PrinterProduct" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<PrinterProduct> PrinterProducts => Set<PrinterProduct>();

        /// <summary>
        /// Gets a collection of <see cref="RemotePrintQueue" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<RemotePrintQueue> RemotePrintQueues => Set<RemotePrintQueue>();

        /// <summary>
        /// Gets a collection of <see cref="AssetInventory.ReservationHistory" /> objects from the Asset Inventory database.
        /// </summary>
        public DbSet<ReservationHistory> ReservationHistory => Set<ReservationHistory>();

        /// <summary>
        /// Configures the entity framework model for the context.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Do not use EF pluralizing conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Define Table-Per-Type inheritance
            modelBuilder.Entity<Camera>().ToTable("Camera");
            modelBuilder.Entity<DeviceSimulator>().ToTable("DeviceSimulator");
            modelBuilder.Entity<MobileDevice>().ToTable("MobileDevice");
            modelBuilder.Entity<Printer>().ToTable("Printer");
            modelBuilder.Entity<VirtualPrinter>().ToTable("VirtualPrinter");

            // Declare primary keys for cases where EF will not infer them
            modelBuilder.Entity<AssetPool>().HasKey(n => n.Name);
            modelBuilder.Entity<CitrixPublishedApp>().HasKey(n => new { n.CitrixServer, n.ApplicationName });
            modelBuilder.Entity<DomainAccountPool>().HasKey(n => n.DomainAccountKey);
            modelBuilder.Entity<DomainAccountReservation>().HasKey(n => new { n.DomainAccountKey, n.SessionId });
            modelBuilder.Entity<FrameworkClient>().HasKey(n => n.FrameworkClientHostName);
            modelBuilder.Entity<LicenseOwner>().HasKey(n => new { n.LicenseId, n.Contact });
            modelBuilder.Entity<PrinterProduct>().HasKey(n => new { n.Family, n.Name });
            modelBuilder.Entity<ReservationHistory>().HasKey(n => n.ReservationHistoryId);
            modelBuilder.Entity<ServerSetting>().HasKey(n => new { n.FrameworkServerId, n.Name });

            // Declare one-to-many and many-to-many foreign key associations
            modelBuilder.Entity<Asset>().HasRequired(n => n.Pool).WithMany(n => n.Assets).Map(n => n.MapKey("PoolName"));
            modelBuilder.Entity<FrameworkClient>().HasMany(n => n.Platforms).WithMany().Map(n => n.ToTable("FrameworkClientPlatformAssoc").MapLeftKey("FrameworkClientHostName").MapRightKey("FrameworkClientPlatformId"));
            modelBuilder.Entity<FrameworkServer>().HasMany(n => n.ServerTypes).WithMany().Map(n => n.ToTable("FrameworkServerTypeAssoc").MapLeftKey("FrameworkServerId").MapRightKey("FrameworkServerTypeId"));
            modelBuilder.Entity<PrintDriver>().HasRequired(n => n.PrintDriverPackage).WithMany(n => n.PrintDrivers).Map(n => n.MapKey("PrintDriverPackageId"));
            modelBuilder.Entity<PrintDriverConfig>().HasMany(n => n.PrintDriverProducts).WithMany().Map(n => n.ToTable("PrintDriverProductAssoc").MapLeftKey("PrintDriverConfigId").MapRightKey("PrintDriverProductId"));
            modelBuilder.Entity<PrintDriverConfig>().HasMany(n => n.PrintDriverVersions).WithMany().Map(n => n.ToTable("PrintDriverVersionAssoc").MapLeftKey("PrintDriverConfigId").MapRightKey("PrintDriverVersionId"));
            modelBuilder.Entity<RemotePrintQueue>().HasRequired(n => n.PrintServer).WithMany().HasForeignKey(n => n.PrintServerId);
        }
    }
}
