namespace NaradX.API.Extensions
{
    public static class ApiServiceExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddHttpClient();

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
