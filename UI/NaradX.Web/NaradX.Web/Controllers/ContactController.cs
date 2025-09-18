using Microsoft.AspNetCore.Mvc;
using NaradX.Entities.Common;
using NaradX.Entities.Response.Contact;
using NaradX.Web.Security.Interfaces;

namespace NaradX.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IApiHelper _apiHelper;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IApiHelper apiHelper, ILogger<ContactController> logger)
        {
            _apiHelper = apiHelper;
            _logger = logger;
        }

        [Route("manage-contacts")]
        public async Task<IActionResult> ManageContacts(int pageNumber = 1, int pageSize = 10, string search = null)
        {
            var endpoint = $"api/Contact/contact-list?tenantId=1&pageNumber={pageNumber}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(search))
            {
                endpoint += $"&search={Uri.EscapeDataString(search)}";
            }
            var contacts = await _apiHelper.GetData<PaginatedList<ContactDto>>(endpoint);

            return View(contacts);
        }

    }
}
