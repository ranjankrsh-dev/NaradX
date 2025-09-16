using Microsoft.AspNetCore.Mvc;

namespace NaradX.Web.Controllers
{
    public class ContactController : Controller
    {
        [Route("manage-contacts")]
        public IActionResult ManageContacts()
        {
            return View();
        }
    }
}
