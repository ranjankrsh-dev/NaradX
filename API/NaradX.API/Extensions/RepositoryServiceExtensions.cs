// <copyright file="RepositoryServiceExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.API.Extensions
{
    using NaradX.Domain.Entities.Auth;
    using NaradX.Domain.Interfaces;
    using NaradX.Infrastructure.Repositories;

    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            // Generic Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Specific Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();

            // Add new repositories here as they are created
            // services.AddScoped<ICampaignRepository, CampaignRepository>();
            // services.AddScoped<ITemplateRepository, TemplateRepository>();

            services.AddScoped<IRepository<RefreshToken>, Repository<RefreshToken>>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
