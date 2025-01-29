using Microsoft.AspNetCore.Mvc;

namespace AustraliaSays2.wwwroot
{
    public class SiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
