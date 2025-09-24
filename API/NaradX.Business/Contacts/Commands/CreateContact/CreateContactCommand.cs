using MediatR;

namespace NaradX.Business.Contacts.Commands.CreateContact
{
    public class CreateContactCommand : IRequest<int>
    {
        public int TenantId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public int CountryId { get; set; }
        public int LanguageId { get; set; }
        public string ContactSource { get; set; } = null!;
        public string ChannelPreference { get; set; } = null!;
        public string? Email { get; set; }
        public string? Company { get; set; }
        public string? JobTitle { get; set; }
    }
}
