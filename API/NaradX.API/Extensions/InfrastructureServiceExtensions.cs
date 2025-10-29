using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NaradX.Business.Common.Interfaces;
using NaradX.Business.Common.Models;
using NaradX.Business.Common.Services;
using NaradX.Infrastructure;
using System;
using System.Text;

namespace NaradX.API.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<NaradXDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Caching (Redis, MemoryCache, etc.)
            services.AddDistributedMemoryCache();
            // services.AddStackExchangeRedisCache(...);

            // External Services (WhatsApp API, SMS, Email)
            // services.AddHttpClient<IWhatsAppService, WhatsAppService>();
            // services.AddScoped<IEmailService, EmailService>();

            // JWT Configuration
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ICommonServices, CommonServices>();




            return services;
        }
    }
}
