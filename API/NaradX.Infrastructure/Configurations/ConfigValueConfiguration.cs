// <copyright file="ConfigValueConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Infrastructure.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using NaradX.Domain.Entities.Common;

    public class ConfigValueConfiguration : IEntityTypeConfiguration<ConfigValue>
    {
        public void Configure(EntityTypeBuilder<ConfigValue> builder)
        {
            // Table name and schema
            builder.ToTable("ConfigValues");

            // Primary Key
            builder.HasKey(cv => cv.Id);

            // Properties
            builder.Property(cv => cv.ItemValue)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(cv => cv.ItemText)
                   .IsRequired()
                   .HasMaxLength(255);

            // Relationships
            builder.HasOne(cv => cv.ConfigMaster)
                   .WithMany(cm => cm.ConfigValues)
                   .HasForeignKey(cv => cv.ConfigMasterId)
                   .OnDelete(DeleteBehavior.Cascade); // If a ConfigMaster is deleted, delete its values

            // Composite Index for efficient querying
            // This index is crucial for the performance of our retrieval logic
            builder.HasIndex(cv => new { cv.ConfigMasterId, cv.TenantId, cv.ItemValue })
                   .IsUnique()
                   .HasFilter("[IsActive] = 1"); // Unique combination per active record

            builder.HasIndex(cv => new { cv.ConfigMasterId, cv.TenantId, cv.DisplayOrder }); // For ordering
        }
    }
}
