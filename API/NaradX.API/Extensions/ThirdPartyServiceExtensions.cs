using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NaradX.Business.Common.Models;
using System.Text;

namespace NaradX.API.Extensions
{
    public static class ThirdPartyServiceExtensions
    {
        public static IServiceCollection AddThirdPartyServices(this IServiceCollection services, IConfiguration configuration)
        {
            // JWT Authentication
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Health Checks
            services.AddHealthChecks();
            // services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

            // Authentication & Authorization (JWT, etc.)
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)...;
            // services.AddAuthorization();
            services.AddAuthorization(options =>
            {
                // Role-Based Policies
                options.AddPolicy("RequireSuperAdmin", policy =>
                    policy.RequireRole("SuperAdmin"));

                options.AddPolicy("RequireTenantAdmin", policy =>
                    policy.RequireRole("TenantAdmin", "SuperAdmin"));

                options.AddPolicy("RequireAdmin", policy =>
                    policy.RequireRole("SuperAdmin", "TenantAdmin"));

                // Permission-Based Policies
                options.AddPolicy("CanManageRoles", policy =>
                    policy.RequireClaim("permission", "ROLE_CREATE", "ROLE_UPDATE", "ROLE_DELETE", "ROLE_ASSIGN"));

                options.AddPolicy("CanAssignRoles", policy =>
                    policy.RequireClaim("permission", "ROLE_ASSIGN"));

                options.AddPolicy("CanManageUsers", policy =>
                    policy.RequireClaim("permission", "USER_CREATE", "USER_UPDATE", "USER_DELETE"));

                options.AddPolicy("CanViewUsers", policy =>
                    policy.RequireClaim("permission", "USER_READ"));

                options.AddPolicy("CanManagePermissions", policy =>
                    policy.RequireClaim("permission", "PERMISSION_ASSIGN"));

                options.AddPolicy("CanViewPermissions", policy =>
                    policy.RequireClaim("permission", "PERMISSION_READ"));

                options.AddPolicy("CanManageTenants", policy =>
                    policy.RequireClaim("permission", "TENANT_CREATE", "TENANT_UPDATE", "TENANT_DELETE"));

                options.AddPolicy("CanViewTenants", policy =>
                    policy.RequireClaim("permission", "TENANT_READ"));

                // Composite Policies (Role + Permission)
                options.AddPolicy("FullSystemAccess", policy =>
                    policy.RequireRole("SuperAdmin").RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "permission")));
            });

            // Rate Limiting
            // services.AddRateLimiter(...);

            return services;
        }
    }
}
