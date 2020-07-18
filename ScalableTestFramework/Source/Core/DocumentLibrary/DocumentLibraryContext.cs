using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HP.ScalableTest.Core.DocumentLibrary
{
    /// <summary>
    /// Entity framework context for connecting to the Document Library database.
    /// </summary>
    public sealed class DocumentLibraryContext : DbContext
    {
        /// <summary>
        /// Initializes the <see cref="DocumentLibraryContext" /> class.
        /// </summary>
        static DocumentLibraryContext()
        {
            // Disable code-first migration and database creation
            Database.SetInitializer<DocumentLibraryContext>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentLibraryContext" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="DocumentLibraryConnectionString" />.</param>
        public DocumentLibraryContext(DocumentLibraryConnectionString connectionString)
            : base(connectionString.ToString())
        {
        }

        /// <summary>
        /// Gets a collection of <see cref="TestDocument" /> objects from the Document Library database.
        /// </summary>
        public DbSet<TestDocument> TestDocuments => Set<TestDocument>();

        /// <summary>
        /// Gets a collection of <see cref="TestDocumentExtension" /> objects from the Document Library database.
        /// </summary>
        public DbSet<TestDocumentExtension> TestDocumentExtensions => Set<TestDocumentExtension>();

        /// <summary>
        /// Gets a collection of <see cref="TestDocumentSet" /> objects from the Document Library database.
        /// </summary>
        public DbSet<TestDocumentSet> TestDocumentSets => Set<TestDocumentSet>();

        /// <summary>
        /// Configures the entity framework model for the context.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Do not use EF pluralizing conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Declare primary keys for cases where EF will not infer them
            modelBuilder.Entity<TestDocumentExtension>().HasKey(n => n.Extension);
            modelBuilder.Entity<TestDocumentSet>().HasKey(n => n.TestDocumentSetId);
            modelBuilder.Entity<TestDocumentSetItem>().HasKey(n => new { n.TestDocumentSetId, n.TestDocumentId });
        }
    }
}
