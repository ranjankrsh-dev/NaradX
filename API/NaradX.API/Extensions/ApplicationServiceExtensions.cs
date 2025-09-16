using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NaradX.Business;
using NaradX.Business.Auth.Register;
using NaradX.Business.Common.Interfaces;
using NaradX.Business.Common.Services;
using AutoMapper;

namespace NaradX.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(ApplicationAssembly).Assembly));

            // Register FluentValidation
            services.AddValidatorsFromAssembly(typeof(ApplicationAssembly).Assembly);

            // Register AutoMapper

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            return services;
        }
    }
}
