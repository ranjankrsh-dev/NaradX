using Microsoft.AspNetCore.Mvc;
using NaradX.Entities.Common;
using NaradX.Entities.Response.Contact;
using NaradX.Web.Models.Contact;
using NaradX.Web.Services.Interfaces.Common;
using NaradX.Web.Services.Interfaces.Contact;
using NaradX.Web.ViewModels.Contact;

namespace NaradX.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IApiHelper apiHelper;
        private readonly ILogger<ContactController> logger;
        private readonly IConfigValueService configValueService;
        private readonly IContactService contactService;

        public ContactController(
            IApiHelper apiHelper, 
            ILogger<ContactController> logger, 
            IConfigValueService configValueService, 
            IContactService contactService)
        {
            this.apiHelper = apiHelper;
            this.logger = logger;
            this.configValueService = configValueService;
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

            ContactViewModel model = new ContactViewModel()
            {
                ContactList = await contactService.GetContactsAsync(filters),
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

            return View(model);
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
    }
}