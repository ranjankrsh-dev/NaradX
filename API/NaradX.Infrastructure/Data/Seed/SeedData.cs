using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NaradX.Domain.Entities.Auth;
using NaradX.Domain.Entities.Common;
using NaradX.Domain.Entities.Tenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Infrastructure.Data.Seed
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new NaradXDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<NaradXDbContext>>()))
            {
                // Seed Roles
                SeedRoles(context);

                // Seed Permissions
                SeedPermissions(context);

                // Seed Role-Permission mappings
                SeedRolePermissions(context);

                // Seed Tenants for SuperAdmin User
                SeedTenantsForSuperAdminUser(context);

                // Seed SuperAdmin User (if not exists)
                SeedSuperAdminUser(context);

                // Seed Config Masters and Values
                SeedConfigMasterAndValues(context);
            }
        }

        private static void SeedRoles(NaradXDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role
                    {
                        Name = "SuperAdmin",
                        Description = "Full system administrator with access to all tenants and features",
                        IsSystemRole = true,
                        CreatedBy = "System",
                        CreatedOn = DateTime.UtcNow
                    },
                    new Role
                    {
                        Name = "TenantAdmin",
                        Description = "Administrator for a specific tenant",
                        IsSystemRole = true,
                        CreatedBy = "System",
                        CreatedOn = DateTime.UtcNow
                    },
                    new Role
                    {
                        Name = "User",
                        Description = "Regular user with basic permissions",
                        IsSystemRole = true,
                        CreatedBy = "System",
                        CreatedOn = DateTime.UtcNow
                    }
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }
        }

        private static void SeedPermissions(NaradXDbContext context)
        {
            if (!context.Permissions.Any())
            {
                var permissions = new List<Permission>
                {
                    // Role Management
                    new Permission { Name = "Role.Assign", Description = "Assign roles to users", Code = "ROLE_ASSIGN", Category = "RoleManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new Permission { Name = "Role.Create", Description = "Create new roles", Code = "ROLE_CREATE", Category = "RoleManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new Permission { Name = "Role.Read", Description = "View roles", Code = "ROLE_READ", Category = "RoleManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new Permission { Name = "Role.Update", Description = "Update roles", Code = "ROLE_UPDATE", Category = "RoleManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new Permission { Name = "Role.Delete", Description = "Delete roles", Code = "ROLE_DELETE", Category = "RoleManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },

                    // User Management
                    new Permission { Name = "User.Create", Description = "Create new users", Code = "USER_CREATE", Category = "UserManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new Permission { Name = "User.Read", Description = "View users", Code = "USER_READ", Category = "UserManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new Permission { Name = "User.Update", Description = "Update users", Code = "USER_UPDATE", Category = "UserManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new Permission { Name = "User.Delete", Description = "Delete users", Code = "USER_DELETE", Category = "UserManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },

                    // Permission Management
                    new Permission { Name = "Permission.Read", Description = "View permissions", Code = "PERMISSION_READ", Category = "PermissionManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new Permission { Name = "Permission.Assign", Description = "Assign permissions to roles", Code = "PERMISSION_ASSIGN", Category = "PermissionManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },

                    // Tenant Management
                    new Permission { Name = "Tenant.Create", Description = "Create tenants", Code = "TENANT_CREATE", Category = "TenantManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new Permission { Name = "Tenant.Read", Description = "View tenants", Code = "TENANT_READ", Category = "TenantManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new Permission { Name = "Tenant.Update", Description = "Update tenants", Code = "TENANT_UPDATE", Category = "TenantManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new Permission { Name = "Tenant.Delete", Description = "Delete tenants", Code = "TENANT_DELETE", Category = "TenantManagement", IsEnabled = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow }
                };

                context.Permissions.AddRange(permissions);
                context.SaveChanges();
            }
        }

        private static void SeedRolePermissions(NaradXDbContext context)
        {
            if (!context.RolePermissions.Any())
            {
                var superAdminRole = context.Roles.FirstOrDefault(r => r.Name == "SuperAdmin");
                var allPermissions = context.Permissions.ToList();

                if (superAdminRole != null && allPermissions.Any())
                {
                    var rolePermissions = allPermissions.Select(p => new RolePermission
                    {
                        RoleId = superAdminRole.Id,
                        PermissionId = p.Id,
                        CreatedBy = "System",
                        CreatedOn = DateTime.UtcNow
                    }).ToList();

                    context.RolePermissions.AddRange(rolePermissions);
                    context.SaveChanges();
                }
            }
        }

        private static void SeedSuperAdminUser(NaradXDbContext context)
        {
            if (!context.Users.Any(u => u.Email == "ranjansharma.cs@gmail.com"))
            {
                var superAdminRole = context.Roles.FirstOrDefault(r => r.Name == "SuperAdmin");

                if (superAdminRole != null)
                {
                    var superAdminUser = new User
                    {
                        Email = "ranjansharma.cs@gmail.com",
                        FirstName = "Ranjan",
                        LastName = "Sharma",
                        PasswordHash = System.Text.Encoding.UTF8.GetBytes(BCrypt.Net.BCrypt.HashPassword("Admin@123")),
                        TenantId = 1,
                        PhoneNumber= "1234567890",
                        EmailVerified = true,
                        EmailVerifiedAt = DateTime.UtcNow,
                        IsActive = true,
                        CreatedBy = "System",
                        CreatedOn = DateTime.UtcNow
                    };

                    context.Users.Add(superAdminUser);
                    context.SaveChanges();

                    // Assign SuperAdmin role
                    var userRole = new UserRole
                    {
                        UserId = superAdminUser.Id,
                        RoleId = superAdminRole.Id,
                        CreatedBy = "System",
                        CreatedOn = DateTime.UtcNow
                    };

                    context.UserRoles.Add(userRole);
                    context.SaveChanges();
                }
            }
        }

        private static void SeedTenantsForSuperAdminUser(NaradXDbContext context)
        {
            if (!context.Tenants.Any())
            {
                var tenants = new List<Tenant>
                {
                    new Tenant
                    {
                        Name = "Default Tenant",
                        Description = "This is the default tenant.",
                        IsActive = true,
                        CreatedBy = "System",
                        CreatedOn = DateTime.UtcNow
                    }
                };
                context.Tenants.AddRange(tenants);
                context.SaveChanges();
            }
        }

        private static void SeedConfigMasterAndValues(NaradXDbContext context)
        {
            if (!context.ConfigMasters.Any())
            {
                // 1. Seed the ConfigMaster entries
                var dataSourceMaster = new ConfigMaster
                {
                    ConfigKey = "DATA_SOURCE",
                    Description = "Source of imported data",
                    IsTenantSpecific = false,
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedOn = DateTime.UtcNow
                };

                var contactSourceMaster = new ConfigMaster
                {
                    ConfigKey = "CONTACT_SOURCE",
                    Description = "How a contact was acquired",
                    IsTenantSpecific = true,
                    IsActive = true,
                    CreatedBy = "System",
                    CreatedOn = DateTime.UtcNow
                };

                context.ConfigMasters.AddRange(dataSourceMaster, contactSourceMaster);
                context.SaveChanges();


                // 2. Seed the ConfigValue entries (GLOBAL DEFAULTS)
                // For DATA_SOURCE
                var dataSourceValues = new List<ConfigValue>
                {
                    new ConfigValue { ConfigMasterId = dataSourceMaster.Id, TenantId = null, ItemValue = "API", ItemText = "API", DisplayOrder = 1, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new ConfigValue { ConfigMasterId = dataSourceMaster.Id, TenantId = null, ItemValue = "MANUAL_UI", ItemText = "Manual UI", DisplayOrder = 2, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new ConfigValue { ConfigMasterId = dataSourceMaster.Id, TenantId = null, ItemValue = "CSV_IMPORT", ItemText = "CSV Import", DisplayOrder = 3, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new ConfigValue { ConfigMasterId = dataSourceMaster.Id, TenantId = null, ItemValue = "EXCEL_IMPORT", ItemText = "Excel Import", DisplayOrder = 4, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow }
                };

                // For CONTACT_SOURCE - GLOBAL FALLBACK VALUES
                var contactSourceValues = new List<ConfigValue>
                {
                    new ConfigValue { ConfigMasterId = contactSourceMaster.Id, TenantId = null, ItemValue = "WEBSITE", ItemText = "Website", DisplayOrder = 1, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new ConfigValue { ConfigMasterId = contactSourceMaster.Id, TenantId = null, ItemValue = "EMAIL", ItemText = "Email", DisplayOrder = 2, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new ConfigValue { ConfigMasterId = contactSourceMaster.Id, TenantId = null, ItemValue = "WALKIN", ItemText = "Walk-In", DisplayOrder = 3, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new ConfigValue { ConfigMasterId = contactSourceMaster.Id, TenantId = null, ItemValue = "FRIENDS", ItemText = "Friends", DisplayOrder = 4, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
                    new ConfigValue { ConfigMasterId = contactSourceMaster.Id, TenantId = null, ItemValue = "SOCIAL_MEDIA", ItemText = "Social Media", DisplayOrder = 5, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.UtcNow }
                };

                context.ConfigValues.AddRange(dataSourceValues);
                context.ConfigValues.AddRange(contactSourceValues);
                context.SaveChanges();
            }
        }

    }
}
