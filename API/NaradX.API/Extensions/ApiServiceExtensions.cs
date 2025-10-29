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

            services.AddAutoMapper(cfg =>
            {
                //cfg.AddProfile<ContactProfile>();
            }, typeof(Program).Assembly);

            // API Versioning (if needed)
            //services.AddApiVersioning();

            return services;
        }
    }
}
