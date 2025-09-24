using NaradX.Entities.Common;
using NaradX.Entities.Response.Contact;
using NaradX.Web.Models.Contact;

namespace NaradX.Web.ViewModels.Contact
{
    public class ContactViewModel
    {
        public ContactDto? Contact { get; set; }
        public PaginatedList<ContactDto>? ContactList { get; set; }
        public ContactFilters? filters { get; set; }
    }
}
