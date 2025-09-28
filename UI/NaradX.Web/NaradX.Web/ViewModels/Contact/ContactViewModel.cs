using NaradX.Entities.Common;
using NaradX.Entities.Response.Common;
using NaradX.Entities.Response.Contact;
using NaradX.Web.Models.Contact;

namespace NaradX.Web.ViewModels.Contact
{
    public class ContactViewModel
    {
        public ContactDto? Contact { get; set; }
        public PaginatedList<ContactDto>? ContactList { get; set; }
        public ContactFilters? Filters { get; set; }
        public IReadOnlyList<CountryDto> Countries { get; set; } = new List<CountryDto>();
        public Dictionary<string, IReadOnlyList<ConfigValueDto>> ConfigValues { get; set; } = new Dictionary<string, IReadOnlyList<ConfigValueDto>>();
    }
}
