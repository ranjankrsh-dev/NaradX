using NaradX.Entities.Common;
using NaradX.Entities.Request.Contact;
using NaradX.Entities.Response.Auth;
using NaradX.Entities.Response.Common;
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

        public async Task<ContactDto> GetContactByIdAsync(int contactId)
        {
            string endpoint = $"{BaseUrl}/get-contact-by-id/{contactId}";
            return await apiHelper.GetData<ContactDto>(endpoint);
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

        public Task<int> UpdateContactAsync(ContactDto contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact));
            }
            string endpoint = $"{BaseUrl}/update";
            return apiHelper.PutData<ContactDto, int>(endpoint, contact);
        }

        public Task<int> DeleteContactAsync(int contactId)
        {
            if(contactId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(contactId), "Contact ID must be greater than zero.");
            }
            string endpoint = $"{BaseUrl}/delete-contact-by-id/{contactId}";
            return apiHelper.GetData<int>(endpoint);
        }

        public async Task<BulkUploadValidateResponseDto> GetBulkUploadValidationResponseAsync(BulkUploadValidateRequest bulkUpload)
        {
            string endpoint = $"{BaseUrl}/bulk-upload-validate";
            return await apiHelper.PostMultipartData<BulkUploadValidateRequest, BulkUploadValidateResponseDto>(endpoint, bulkUpload);
        }

        public async Task<ResponseDto> BulkUploadConfirmAsync(BulkUploadValidateResponseDto bulkUpload)
        {
            string endpoint = $"{BaseUrl}/bulk-upload-confirm";
            return await apiHelper.PostData<BulkUploadValidateResponseDto, ResponseDto>(endpoint, bulkUpload);
        }
    }
}
