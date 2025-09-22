using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaradX.Domain.Entities.ManageContact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Infrastructure.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");

            // Required property configurations
            builder.Property(c => c.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(c => c.ContactSource)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasDefaultValue("Manual");

            // Computed DisplayName
            builder.Property(c => c.DisplayName)
                   .HasMaxLength(500)
                   .HasComputedColumnSql("TRIM(CONCAT([FirstName], ' ', COALESCE([MiddleName] + ' ', ''), [LastName]))", stored: true);

            // Critical indexes only
            builder.HasIndex(c => new { c.TenantId, c.PhoneNumber })
                   .IsUnique()
                   .HasFilter("[IsActive] = 1 AND [IsDeleted] = 0");

            builder.HasIndex(c => new { c.TenantId, c.Email })
                   .HasFilter("[Email] IS NOT NULL AND [IsActive] = 1 AND [IsDeleted] = 0");

            // Relationships
            builder.HasOne(c => c.Tenant)
                   .WithMany(t => t.Contacts)
                   .HasForeignKey(c => c.TenantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Country)
                   .WithMany()
                   .HasForeignKey(c => c.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Language)
                   .WithMany()
                   .HasForeignKey(c => c.LanguageId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
