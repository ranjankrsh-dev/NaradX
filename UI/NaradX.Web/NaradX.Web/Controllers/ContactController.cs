using Microsoft.AspNetCore.Mvc;
using NaradX.Entities.Common;
using NaradX.Entities.Enums.Common;
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
                response = await contactService.AddContactAsync(viewModel.Contact);
            }
            else
            {
                logger.LogError("Invalid model state or contact is null");
            }
            
            return RedirectToAction("ManageContacts");
        }

        [HttpGet]
        public async Task<JsonResult> GetLanguagesByCountry(int countryId)
        {
            var langs = await commonServices.GetLanguagesByCountryIdAsync(countryId);
            return Json(langs);
        }
    }
}