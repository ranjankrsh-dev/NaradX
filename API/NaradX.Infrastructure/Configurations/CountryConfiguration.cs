using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaradX.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Infrastructure.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            // Table name
            builder.ToTable("Countries");

            // Primary Key
            builder.HasKey(c => c.Id);

            // Properties configuration
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Code)
                   .IsRequired()
                   .HasMaxLength(2); // ISO 3166-1 alpha-2 code (e.g., US, IN)

            builder.Property(c => c.PhoneCode)
                   .IsRequired()
                   .HasMaxLength(5); // e.g., +1, +91

            builder.Property(c => c.CurrencyCode)
                   .IsRequired()
                   .HasMaxLength(3); // ISO 4217 code (e.g., USD, INR)

            builder.Property(c => c.CurrencySymbol)
                   .IsRequired()
                   .HasMaxLength(5); // e.g., $, ₹, €

            builder.Property(c => c.Timezone)
                   .IsRequired()
                   .HasMaxLength(50); // e.g., IST, EST

            // Indexes
            builder.HasIndex(c => c.Code)
                   .IsUnique();

            builder.HasIndex(c => c.Name)
                   .IsUnique();
        }
    }
}
