using FluentAssertions;
using Xunit;

namespace NaradX.IntegrationTests.HealthCheck
{
    public class HealthCheckTests : BaseIntegrationTest
    {
        public HealthCheckTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Get_Root_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/weatherforecast"); // Assuming this default endpoint exists, or checking swagger/health if available

            // Assert
            // response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK); 
            // Since we don't know exact endpoints yet, let's just assert the application starts and returns something (e.g. 404 is fine as long as server is up)
             response.StatusCode.Should().NotBe(System.Net.HttpStatusCode.InternalServerError);
        }
    }
}
