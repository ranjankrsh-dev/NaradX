using Microsoft.Extensions.DependencyInjection;
using NaradX.Infrastructure;
using System.Net.Http.Json;
using Xunit;

namespace NaradX.IntegrationTests
{
    public abstract class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IServiceScope _scope;
        protected readonly CustomWebApplicationFactory _factory;
        protected readonly HttpClient _client;
        protected readonly NaradXDbContext _dbContext;

        protected BaseIntegrationTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _scope = factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<NaradXDbContext>();
        }

        protected async Task AuthenticateAsync()
        {
            var loginCommand = new
            {
                Email = "ranjansharma.cs@gmail.com",
                Password = "Admin@123"
            };

            var response = await _client.PostAsJsonAsync("/api/Auth/login", loginCommand);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (result != null)
            {
                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);
            }
        }

        private class LoginResponseDto
        {
            public string AccessToken { get; set; } = string.Empty;
        }
    }
}
