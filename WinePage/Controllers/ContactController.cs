using Microsoft.AspNetCore.Mvc;

namespace WinePage.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
