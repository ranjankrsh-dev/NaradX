using NaradX.Entities.Common;
using NaradX.Entities.Response.Auth;
using NaradX.Entities.Response.Contact;
using NaradX.Web.Models.Contact;
using NaradX.Web.Services.Implementations.Common;
using NaradX.Web.Services.Interfaces.Common;
using NaradX.Web.Services.Interfaces.Contact;

namespace NaradX.Web.Services.Implementations.Contact
{
    public class ContactService : IContactService
    {
        private const string BaseUrl = "api/contact";
        private readonly IApiHelper apiHelper;
        private readonly ILogger<ContactService> logger;
        public ContactService(ILogger<ContactService> logger, IApiHelper apiHelper)
        {
            this.logger = logger;
            this.apiHelper = apiHelper;
        }
        public async Task<PaginatedList<ContactDto>?> GetContactsAsync(ContactFilters filters)
        {
            string endpoint = $"{BaseUrl}/list";
            return await apiHelper.PostData<ContactFilters, PaginatedList<ContactDto>>(endpoint, filters);
        }

        public Task<ContactDto> GetContactByIdAsync(int contactId)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddContactAsync(ContactDto contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            string endpoint = $"{BaseUrl}/add";
            return apiHelper.PostData<ContactDto, int>(endpoint, contact);
        }

        public Task<bool> UpdateContactAsync(int contactId, ContactDto contact)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteContactAsync(int contactId)
        {
            throw new NotImplementedException();
        }
    }
}
