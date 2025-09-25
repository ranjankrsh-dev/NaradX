using NaradX.Entities.Common;
using NaradX.Entities.Response.Contact;
using NaradX.Web.Models.Contact;

namespace NaradX.Web.Services.Interfaces.Contact
{
    public interface IContactService
    {
        Task<PaginatedList<ContactDto>?> GetContactsAsync(ContactFilters filters);
        Task<ContactDto> GetContactByIdAsync(int contactId);
        Task<int> AddContactAsync(ContactDto contact);
        Task<bool> UpdateContactAsync(int contactId, ContactDto contact);
        Task<bool> DeleteContactAsync(int contactId);
    }
}
