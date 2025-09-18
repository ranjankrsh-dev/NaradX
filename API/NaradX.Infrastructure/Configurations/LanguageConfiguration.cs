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
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            // Table name
            builder.ToTable("Languages");

            // Primary Key
            builder.HasKey(l => l.Id);

            // Properties configuration
            builder.Property(l => l.Culture)
                   .IsRequired()
                   .HasMaxLength(10); // e.g., en-US, hi-IN

            builder.Property(l => l.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(l => l.LocalName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(l => l.Description)
                   .HasMaxLength(500);

            // Indexes
            builder.HasIndex(l => l.Culture)
                   .IsUnique();
        }
    }
}
