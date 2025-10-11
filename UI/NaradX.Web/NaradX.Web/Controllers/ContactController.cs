using Microsoft.AspNetCore.Mvc;
using NaradX.Entities.Common;
using NaradX.Entities.Enums.Common;
using NaradX.Entities.Request.Contact;
using NaradX.Entities.Response.Common;
using NaradX.Entities.Response.Contact;
using NaradX.Web.Models.Contact;
using NaradX.Web.Services.Interfaces.Common;
using NaradX.Web.Services.Interfaces.Contact;
using NaradX.Web.ViewModels.Contact;
using System.Text.Json;

namespace NaradX.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IApiHelper apiHelper;
        private readonly ILogger<ContactController> logger;
        private readonly ICommonServices commonServices;
        private readonly IContactService contactService;

        public ContactController(
            IApiHelper apiHelper, 
            ILogger<ContactController> logger,
            ICommonServices commonServices, 
            IContactService contactService)
        {
            this.apiHelper = apiHelper;
            this.logger = logger;
            this.commonServices = commonServices;
            this.contactService = contactService;
        }

        [HttpGet]
        [Route("manage-contacts")]
        public async Task<IActionResult> ManageContacts()
        {
            int tenantId = HttpContext.Session.GetInt32("tenantId") ?? 0;
            ContactFilters filters = new ContactFilters();
            if (tenantId != 0) 
            {
                filters.TenantId = tenantId;
            }
            IEnumerable<string> configKeys = Enum.GetNames(typeof(ConfigKeys)).ToList();
            var aa = await commonServices.GetMultipleConfigValuesAsync(configKeys, tenantId);

            ContactViewModel model = new ContactViewModel()
            {
                ContactList = await contactService.GetContactsAsync(filters),
                Countries = await commonServices.GetAllCountriesAsync(),
                ConfigValues = await commonServices.GetMultipleConfigValuesAsync(Enum.GetNames(typeof(ConfigKeys)).ToList(), tenantId)
            };

            return View(model);
        }

        [HttpPost]
        [Route("manage-contacts")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageContacts(ContactFilters filters)
        {
            filters.TenantId = HttpContext.Session.GetInt32("tenantId") ?? 0;

            ContactViewModel model = new ContactViewModel()
            {
                ContactList = await contactService.GetContactsAsync(filters),
            };

            return PartialView("_ContactTablePartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(ContactViewModel viewModel)
        {
            int response = 0;
            if (ModelState.IsValid && viewModel.Contact != null)
            {
                viewModel.Contact.TenantId = HttpContext.Session.GetInt32("tenantId") ?? 0;
                if (viewModel.RequestType == "Save")
                {
                    response = await contactService.AddContactAsync(viewModel.Contact);
                }
                else if (viewModel.RequestType == "Update" && viewModel.Contact.Id > 0)
                {
                    response = await contactService.UpdateContactAsync(viewModel.Contact);
                }
                else
                {
                    logger.LogError("Invalid model state or contact is null");
                }
            }
            else
            {
                logger.LogError("Invalid model state or contact is null");
            }
            
            return RedirectToAction("ManageContacts");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContact(int contactId)
        {
            int isDeleted = await contactService.DeleteContactAsync(contactId);
            if (isDeleted > 0)
            {
                return Json(new { success = true, message = "Contact deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete contact." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetLanguagesByCountry(int countryId)
        {
            var langs = await commonServices.GetLanguagesByCountryIdAsync(countryId);
            return Json(langs);
        }

        [HttpGet]
        public async Task<IActionResult> GetContactById(int contactId)
        {
            var contact = await contactService.GetContactByIdAsync(contactId);
            return Json(new { success = true, data = contact });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteContactById(int contactId)
        {
            var contact = await contactService.DeleteContactAsync(contactId);
            return Json(new { success = true, data = contact });
        }

        #region Bulk Upload Operation

        [HttpPost]
        public async Task<IActionResult> GetBulkUploadValidations([FromForm] BulkUploadValidateRequest bulkUpload)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    success = false,
                    message = "Validation failed",
                    errors = errors
                });
            }

            bulkUpload.TenantId = HttpContext.Session.GetInt32("tenantId") ?? 0;
            var response = await contactService.GetBulkUploadValidationResponseAsync(bulkUpload);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBulkUploadContacts([FromBody] BulkUploadValidateResponseDto bulkUpload)
        {
            if (bulkUpload == null)
            {
                return BadRequest(new ResponseDto { IsSuccess = false, Message = "No contacts to save." });
            }

            if (string.IsNullOrEmpty(bulkUpload.BatchId))
            {
                return BadRequest(new ResponseDto { IsSuccess = false, Message = "Invalid batch ID." });
            }

            await Task.Delay(2000);
            var response = await contactService.BulkUploadConfirmAsync(bulkUpload);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadTemplateFile()
        {
            string endpoint = "api/contact/bulk-upload-download-template";

            var response = await apiHelper.GetFileAsync(endpoint);

            var stream = await response.Content.ReadAsStreamAsync();

            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";

            var result = new FileStreamResult(stream, contentType);

            if (response.Content.Headers.ContentDisposition != null)
            {
                Response.Headers.Add("Content-Disposition", response.Content.Headers.ContentDisposition.ToString());
            }

            return result;
        }

        #endregion
    }
}