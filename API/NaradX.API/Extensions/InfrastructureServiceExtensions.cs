// <copyright file="InfrastructureServiceExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.API.Extensions
{
    using System.Text;
    using System.Text.Json;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using NaradX.Business.Interfaces;
    using NaradX.Business.Models;
    using NaradX.Infrastructure.Services;
    using NaradX.Domain.Interfaces;
    using NaradX.Infrastructure;
    using NaradX.Infrastructure.Gateways.WhatsApp;
    using Refit;

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
            services.AddScoped<ITemplateRepository, NaradX.Infrastructure.Repositories.TemplateRepository>();

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

            // Configure WhatsApp options
            services.Configure<WhatsAppOptions>(configuration.GetSection("WhatsAppOptions"));
            
            var whatsAppOptions = configuration.GetSection("WhatsAppOptions").Get<WhatsAppOptions>();
    
            // Configure Refit settings
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                })
            };

            // Register WhatsApp API client
            services.AddRefitClient<IWhatsAppApiGateway>(refitSettings)
                .ConfigureHttpClient(c => {
                    c.BaseAddress = new Uri(whatsAppOptions.BaseUrl);
                });

            return services;
        }
    }
}
