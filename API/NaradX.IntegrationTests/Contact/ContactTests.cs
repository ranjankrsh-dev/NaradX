using FluentAssertions;
using NaradX.Business.Contacts.Commands.CreateContact;
using NaradX.Business.Dto.Contact;
using NaradX.Domain.Models.Contact;
using System.Net.Http.Json;
using Xunit;

namespace NaradX.IntegrationTests.Contact
{
    public class ContactTests : BaseIntegrationTest
    {
        public ContactTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task CreateContact_ValidData_ReturnsSuccess()
        {
            // Arrange
            await AuthenticateAsync();

            var contact = new CreateContactCommand
            {
                TenantId = 1,
                FirstName = "Integration",
                MiddleName = "",
                LastName = "Test",
                PhoneNumber = "9876543210",
                Email = "test.contact@example.com",
                LanguageId = 2, // English
                CountryId = 1, // India
                ContactSource = "WEBSITE",
                ChannelPreference = "WHATSAPP"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Contact/add", contact);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK, $"Reason: {content}");
            
            // Verify via LIST
            var listResponse = await _client.PostAsJsonAsync("/api/Contact/list", new ContactFilterParams());
            listResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
