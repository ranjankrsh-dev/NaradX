using FluentAssertions;
using NaradX.Business.Auth.Login;
using System.Net.Http.Json;
using Xunit;
using System.Text.Json;

namespace NaradX.IntegrationTests.Auth
{
    public class AuthenticationTests : BaseIntegrationTest
    {
        public AuthenticationTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginCommand = new LoginCommand
            {
                Email = "ranjansharma.cs@gmail.com",
                Password = "Admin@123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Auth/login", loginCommand);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK, $"Reason: {content}");
            
            var result = JsonSerializer.Deserialize<LoginResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            result.Should().NotBeNull();
            result!.AccessToken.Should().NotBeNullOrEmpty($"Content: {content}");
            result.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginCommand = new LoginCommand
            {
                Email = "ranjansharma.cs@gmail.com",
                Password = "WrongPassword"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Auth/login", loginCommand);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            
            // Check for 401 Unauthorized
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized, $"Reason: {content}");
        }
    }
}
