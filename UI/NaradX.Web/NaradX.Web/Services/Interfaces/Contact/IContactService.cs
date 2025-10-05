using NaradX.Entities.Common;
using NaradX.Entities.Request.Contact;
using NaradX.Entities.Response.Common;
using NaradX.Entities.Response.Contact;
using NaradX.Web.Models.Contact;

namespace NaradX.Web.Services.Interfaces.Contact
{
    public interface IContactService
    {
        Task<PaginatedList<ContactDto>?> GetContactsAsync(ContactFilters filters);
        Task<ContactDto> GetContactByIdAsync(int contactId);
        Task<int> AddContactAsync(ContactDto contact);
        Task<int> UpdateContactAsync(ContactDto contact);
        Task<int> DeleteContactAsync(int contactId);
        Task<BulkUploadValidateResponseDto> GetBulkUploadValidationResponseAsync(BulkUploadValidateRequest bulkUpload);
        Task<ResponseDto> BulkUploadConfirmAsync(BulkUploadValidateResponseDto bulkUpload);
    }
}
