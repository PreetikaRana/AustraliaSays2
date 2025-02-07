using AustraliaSays2.Models;
using AustraliaSays2_DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Diagnostics;

namespace AustraliaSays2.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
        
        {
        private readonly ApplicationDbContext _context;
            private readonly ILogger<HomeController> _logger;

            public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
            {
            _context = context;
                _logger = logger;
            }

            public IActionResult Index()
            {
                return View();
            }

            public IActionResult Privacy()
            {
                return View();
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        public async Task<IActionResult> Search(string query)
        {
            if(string.IsNullOrEmpty(query))
            {
                TempData["ErrorMessage"] = "Please Enter a Search Term";
                return RedirectToAction("Index");
            }

            var articles = await _context.Articles.Where(a=>a.Name.ToLower().Contains(query)|| a.Description.ToLower().Contains(query)||a.Tags.ToLower().Contains(query)).Select(a=> new SearchResult
            {
                Name = a.Name,
                U
            })
        }
        }
    }
