using MySql.Data.EntityFramework;
using System.Data.Common;
using System.Data.Entity;

namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BtfModel : DbContext
    {
        public BtfModel()
            : base()
        {
        }

        public BtfModel(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection,
            contextOwnsConnection)
        {
        }

        public virtual DbSet<auth_group> auth_group { get; set; }
        public virtual DbSet<auth_group_permissions> auth_group_permissions { get; set; }
        public virtual DbSet<auth_permission> auth_permission { get; set; }
        public virtual DbSet<auth_user> auth_user { get; set; }
        public virtual DbSet<auth_user_groups> auth_user_groups { get; set; }
        public virtual DbSet<auth_user_user_permissions> auth_user_user_permissions { get; set; }
        public virtual DbSet<authtoken_token> authtoken_token { get; set; }
        public virtual DbSet<testcase> testcases { get; set; }
        public virtual DbSet<testcase_repo> testcase_repo { get; set; }
        public virtual DbSet<testcase_repo_type> testcase_repo_type { get; set; }

        public virtual DbSet<entity> entities { get; set; }
        public virtual DbSet<entity_types> entity_types { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Properties<DateTime>().Configure(c=>c.HasColumnType("datetime2"));

            modelBuilder.Entity<auth_group>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<auth_group>()
                .HasMany(e => e.auth_group_permissions)
                .WithRequired(e => e.auth_group)
                .HasForeignKey(e => e.group_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<auth_group>()
                .HasMany(e => e.auth_user_groups)
                .WithRequired(e => e.auth_group)
                .HasForeignKey(e => e.group_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<auth_permission>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<auth_permission>()
                .Property(e => e.codename)
                .IsUnicode(false);

            modelBuilder.Entity<auth_permission>()
                .HasMany(e => e.auth_group_permissions)
                .WithRequired(e => e.auth_permission)
                .HasForeignKey(e => e.permission_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<auth_permission>()
                .HasMany(e => e.auth_user_user_permissions)
                .WithRequired(e => e.auth_permission)
                .HasForeignKey(e => e.permission_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<auth_user>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<auth_user>()
                .Property(e => e.last_login)
                .HasPrecision(6);

            modelBuilder.Entity<auth_user>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<auth_user>()
                .Property(e => e.first_name)
                .IsUnicode(false);

            modelBuilder.Entity<auth_user>()
                .Property(e => e.last_name)
                .IsUnicode(false);

            modelBuilder.Entity<auth_user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<auth_user>()
                .Property(e => e.date_joined)
                .HasPrecision(6);

            modelBuilder.Entity<auth_user>()
                .HasMany(e => e.auth_user_groups)
                .WithRequired(e => e.auth_user)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<auth_user>()
                .HasMany(e => e.auth_user_user_permissions)
                .WithRequired(e => e.auth_user)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<auth_user>()
                .HasMany(e => e.authtoken_token)
                .WithRequired(e => e.auth_user)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<auth_user>()
                .HasMany(e => e.testcase_repo)
                .WithRequired(e => e.auth_user)
                .HasForeignKey(e => e.created_by)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<authtoken_token>()
                .Property(e => e.key)
                .IsUnicode(false);

            modelBuilder.Entity<authtoken_token>()
                .Property(e => e.created)
                .HasPrecision(6);

            modelBuilder.Entity<testcase>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<testcase>()
                .Property(e => e.path)
                .IsUnicode(false);

            modelBuilder.Entity<testcase>()
                .Property(e => e.modified_dt)
                .HasPrecision(0);

            modelBuilder.Entity<testcase_repo>()
                .Property(e => e.repo_url)
                .IsUnicode(false);

            modelBuilder.Entity<testcase_repo>()
                .Property(e => e.repo_branch)
                .IsUnicode(false);

            modelBuilder.Entity<testcase_repo>()
                .Property(e => e.modified_dt)
                .HasPrecision(0);

            modelBuilder.Entity<testcase_repo>()
                .HasMany(e => e.testcases)
                .WithRequired(e => e.testcase_repo)
                .HasForeignKey(e => e.repo_ref_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<testcase_repo_type>()
                .Property(e => e.repo_type)
                .IsUnicode(false);

            modelBuilder.Entity<testcase_repo_type>()
                .Property(e => e.repo_type_desc)
                .IsUnicode(false);

            modelBuilder.Entity<testcase_repo_type>()
                .Property(e => e.modified_dt)
                .HasPrecision(0);

            modelBuilder.Entity<testcase_repo_type>()
                .HasMany(e => e.testcase_repo)
                .WithRequired(e => e.testcase_repo_type)
                .HasForeignKey(e => e.repo_type_ref_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<entity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<entity>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<entity>()
                .Property(e => e.modified_dt)
                .HasPrecision(0);

            modelBuilder.Entity<entity_types>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<entity_types>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<entity_types>()
                .Property(e => e.modified_dt)
                .HasPrecision(0);

            modelBuilder.Entity<entity_types>()
                .HasMany(e => e.entities)
                .WithRequired(e => e.entity_types)
                .HasForeignKey(e => e.entity_type_id_ref)
                .WillCascadeOnDelete(false);
        }
    }
}