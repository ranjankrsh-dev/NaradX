using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaradX.Domain.Entities.Tenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Infrastructure.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(t => t.Name)
                .IsUnique();

            builder.Property(t => t.Domain)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(t => t.Domain)
                .IsUnique();

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            builder.Property(t => t.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(t => t.ContactEmail)
                .HasMaxLength(100);

            builder.Property(t => t.Address)
                .HasMaxLength(200);

            // Relationships
            builder.HasMany(t => t.Users)
                .WithOne(u => u.Tenant)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
