using AustraliaSays2.Models;
using AustraliaSays2_DataAccess.Data;
using AustraliaSays2_DataAccess.Repository;
using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AustraliaSays2.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
        
        {
        private readonly HomeRepository _homerepository;
            private readonly ILogger<HomeController> _logger;

            public HomeController(ILogger<HomeController> logger, HomeRepository homerepository)
            {
            _homerepository = homerepository;
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

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            // If no query is provided, return an empty list
            if (string.IsNullOrWhiteSpace(query))
            {
                return View("Search", new List<SearchResult>());
            }

            // Fetch search results
            var results = await _homerepository.SearchAsync(query);

            // If the request is for autocomplete (AJAX), return JSON results
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(results);
            }

            // Render the search view with results for non-AJAX requests
            return View("Search", results);
        }
        public async Task<IActionResult> ArticlesByType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return RedirectToAction("Index");
            }

            var articles = await _homerepository.GetArticlesByTypeAsync(type);
            return View(articles);
        }



    }
}
