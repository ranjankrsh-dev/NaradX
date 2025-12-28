using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NaradX.Infrastructure;
using NaradX.Infrastructure.Data.Seed;
using Testcontainers.MsSql;
using Moq;

namespace NaradX.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<NaradXDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<NaradXDbContext>(options =>
                {
                    options.UseSqlServer(_msSqlContainer.GetConnectionString());
                });

                // Mock External Services
                var mockWhatsApp = new Moq.Mock<NaradX.Infrastructure.Gateways.WhatsApp.IWhatsAppApiGateway>();
                mockWhatsApp.Setup(x => x.CreateTemplateAsync(
                    Moq.It.IsAny<string>(), 
                    Moq.It.IsAny<NaradX.Business.Dto.Template.WhatsAppMessageTemplateDTO>(), 
                    Moq.It.IsAny<string>(),
                    Moq.It.IsAny<string>()))
                    .ReturnsAsync(new NaradX.Business.Models.CreateTemplateResponse());

                // Remove existing and add mock
                var whatsappDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(NaradX.Infrastructure.Gateways.WhatsApp.IWhatsAppApiGateway));
                if (whatsappDescriptor != null) services.Remove(whatsappDescriptor);
                services.AddSingleton(mockWhatsApp.Object);
            });
        }

        public async Task InitializeAsync()
        {
            await _msSqlContainer.StartAsync();

            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<NaradXDbContext>();
            await context.Database.MigrateAsync();
            SeedData.Initialize(scope.ServiceProvider);
        }

        public new async Task DisposeAsync()
        {
            await _msSqlContainer.DisposeAsync();
        }
    }
}
