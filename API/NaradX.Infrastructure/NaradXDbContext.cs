using Microsoft.EntityFrameworkCore;
using NaradX.Domain.Entities.Auth;
using NaradX.Domain.Entities.Common;
using NaradX.Domain.Entities.ManageContact;
using NaradX.Domain.Entities.Template;
using NaradX.Domain.Entities.Tenancy;
using NaradX.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Infrastructure
{
    public class NaradXDbContext : DbContext
    {
        public NaradXDbContext(DbContextOptions<NaradXDbContext> options) : base(options)
        {
        }

        // DbSets will be added here
        public DbSet<User> Users { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ChannelPreference> ChannelPreferences { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ContactTag> ContactTags { get; set; }
        public DbSet<ConfigMaster> ConfigMasters { get; set; }
        public DbSet<ConfigValue> ConfigValues { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
        .HasOne(ur => ur.Role)
        .WithMany()
        .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId);

            // Apply all configurations from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Apply global query filters for soft deletable entities
            ApplyGlobalFilters(modelBuilder);

            // Configure many-to-many relationship between Country and Language
            ConfigureCountryLanguageRelationship(modelBuilder);
        }

        private void ApplyGlobalFilters(ModelBuilder modelBuilder)
        {
            // Soft deletable entities filter
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var body = Expression.Equal(
                        Expression.Property(parameter, nameof(ISoftDeletableEntity.IsDeleted)),
                        Expression.Constant(false));

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditableEntity &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            var currentTime = DateTime.UtcNow;
            // Will replace with actual user service later
            var currentUserId = "system";

            foreach (var entry in entries)
            {
                var entity = (IAuditableEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = currentTime;
                    entity.CreatedBy = currentUserId;
                }
                else
                {
                    entity.UpdatedOn = currentTime;
                    entity.UpdatedBy = currentUserId;
                    entry.Property(nameof(IAuditableEntity.CreatedOn)).IsModified = false;
                    entry.Property(nameof(IAuditableEntity.CreatedBy)).IsModified = false;
                }
            }
        }

        private void ConfigureCountryLanguageRelationship(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>()
                .HasOne(l => l.Country)
                .WithMany(c => c.Languages)
                .HasForeignKey(l => l.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
