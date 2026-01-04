using FluentAssertions;
using NaradX.Business.Dto.Template;
using System.Net.Http.Json;
using Xunit;

namespace NaradX.IntegrationTests.Template
{
    public class TemplateTests : BaseIntegrationTest
    {
        public TemplateTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task CreateTemplate_ValidData_ReturnsSuccess()
        {
            // Arrange
            await AuthenticateAsync();

            var template = new WhatsAppMessageTemplateDTO
            {
                Name = "integration_test_template",
                Category = "MARKETING",
                Language = "en_US",
                Components = new List<ComponentDTO>
                {
                    new ComponentDTO
                    {
                        Type = "BODY",
                        Text = "Hello {{1}}, this is a test template."
                    }
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Template", template);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK, $"Reason: {content}");
            
            // Optional: Verify it exists via GET
            var getResponse = await _client.GetAsync($"/api/Template/{template.Name}");
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
