using NaradX.Domain.Entities.Auth;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Infrastructure.Repositories;

namespace NaradX.API.Extensions
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Generic Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Specific Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IRepository<RefreshToken>, Repository<RefreshToken>>();

            return services;
        }
    }
}
