// <copyright file="ApiServiceExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.API.Extensions
{
    public static class ApiServiceExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // API Controllers
            services.AddControllers();

            // API Documentation
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddMemoryCache();

            // API Versioning (if needed)
            // services.AddApiVersioning();

            return services;
        }
    }
}
