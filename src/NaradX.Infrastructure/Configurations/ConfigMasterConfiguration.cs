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
    public class ConfigMasterConfiguration : IEntityTypeConfiguration<ConfigMaster>
    {
        public void Configure(EntityTypeBuilder<ConfigMaster> builder)
        {
            // Table name and schema
            builder.ToTable("ConfigMasters"); // Creates table in "config" schema

            // Primary Key
            builder.HasKey(cm => cm.Id);

            // Properties configuration
            builder.Property(cm => cm.ConfigKey)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(cm => cm.Description)
                   .HasMaxLength(1000);

            // Indexes
            builder.HasIndex(cm => cm.ConfigKey)
                   .IsUnique()
                   .HasFilter("[IsActive] = 1"); // Creates a filtered unique index
        }
    }
}
