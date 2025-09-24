using Microsoft.AspNetCore.Mvc;
using NaradX.Entities.Common;
using NaradX.Entities.Response.Contact;
using NaradX.Web.Models.Contact;
using NaradX.Web.Services.Interfaces.Common;
using NaradX.Web.ViewModels.Contact;

namespace NaradX.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IApiHelper _apiHelper;
        private readonly IConfigValueService _configValueService;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IApiHelper apiHelper, ILogger<ContactController> logger, IConfigValueService configValueService)
        {
            _apiHelper = apiHelper;
            _logger = logger;
            _configValueService = configValueService;
        }

        [HttpGet]
        [Route("manage-contacts")]
        public async Task<IActionResult> ManageContacts(
            int pageNumber = 1, 
            int pageSize = 10,
            string? search = null,
            string? name = null,
            string? phone = null,
            string? status = null,
            string sortColumn = null,
            string sortDirection = "asc"
            )
        {
            var endpoint = $"api/Contact/list?tenantId=1&pageNumber={pageNumber}&pageSize={pageSize}"
                + (string.IsNullOrEmpty(search) ? "" : $"&search={Uri.EscapeDataString(search)}")
                + (string.IsNullOrEmpty(name) ? "" : $"&name={Uri.EscapeDataString(name)}")
                + (string.IsNullOrEmpty(phone) ? "" : $"&phone={Uri.EscapeDataString(phone)}")
                + (string.IsNullOrEmpty(status) ? "" : $"&status={Uri.EscapeDataString(status)}")
                + (string.IsNullOrEmpty(sortColumn) ? "" : $"&sortColumn={Uri.EscapeDataString(sortColumn)}")
                + (string.IsNullOrEmpty(sortDirection) ? "" : $"&sortDirection={Uri.EscapeDataString(sortDirection)}");

            var contacts = await _apiHelper.GetData<PaginatedList<ContactDto>>(endpoint);

            ContactViewModel model = new ContactViewModel
            {
                ContactList = contacts
            };

            return View(model);
        }

        [HttpPost]
        [Route("manage-contacts")]
        public async Task<IActionResult> ManageContacts(ContactFilters filters)
        {
            var endpoint = $"api/Contact/contact-list?tenantId=1&pageNumber=1&pageSize=10";
            var contacts = await _apiHelper.GetData<PaginatedList<ContactDto>>(endpoint);

            ContactViewModel model = new ContactViewModel
            {
                ContactList = contacts
            };

            return View(model);
        }

        [HttpPost]
        [Route("add-contact")]
        public async Task<IActionResult> AddContact(ContactDto contact)
        {
            //if (ModelState.IsValid)
            //{
            //    string apiURL = "api/Auth/login";
            //    var response = await _apiHelper.PostData<object, ContactDto>(apiURL, apiRequest);
            //}
            //if (response != null)
            //{
            //    return RedirectToAction("ManageContacts");
            //}
            //else
            //{
            //    ModelState.AddModelError(string.Empty, "An error occurred while adding the contact.");
            //    return View(contact);
            //}
            return RedirectToAction("ManageContacts");
        }
    }
}