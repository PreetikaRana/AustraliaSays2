using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models.DTO;
using AustraliaSays2_Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace AustraliaSays2.Areas.Admin.Contollers
{
    [Area("Admin")]
    public class SiteController : Controller
    {
        private readonly ISiteRepository _siteRepository;
        public SiteController(ISiteRepository siteRepository)
        {
                _siteRepository = siteRepository;
        }
        public async Task<IActionResult> Index()
        {
           var sites = await _siteRepository.GetAllSitesAsync();
            if (sites == null || !sites.Any())
            {
                Console.WriteLine("No sites found."); // Temporary logging
            }
            else
            {
                foreach (var site in sites)
                {
                    Console.WriteLine($"Site: {site.URL}, Logo: {site.Logo}");
                }
            }

            return View(sites);
        }
        public IActionResult Insert()
        {
            return View(new SiteDTO()); 
        }
        [HttpPost]
        public async Task<IActionResult> Insert(SiteDTO dto)
        {
            
            if (ModelState.IsValid) 
            {
                var site = await _siteRepository.AddSiteAsync(dto);
                var sites = await _siteRepository.GetAllSitesAsync();
                return View("Index", sites);
            }
            return View(dto);
        }
    }
}
