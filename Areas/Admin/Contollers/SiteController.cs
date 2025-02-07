using AustraliaSays2_DataAccess.Repository;
using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models.DTO;
using AustraliaSays2_Models.Models;
using AustraliaSays2_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AustraliaSays2.Areas.Admin.Contollers
{
    [Area("Admin")]
  //  [Authorize]
   
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
                sites = new List<Site>();
            }

            return View(sites);
        }
     //   [ValidateAntiForgeryToken]
        public async Task<IActionResult> Insert(SiteDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _siteRepository.AddSiteAsync(dto);
                    if (dto.Id == null || dto.Id == 0)
                    {
                        TempData["SuccessMessage"] = result != null ? "Site Added Successfully" : "Failed to Add Site";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Site Updated Successfully";
                    }
                    return RedirectToAction("Index"); // Redirect ensures Index view is loaded with proper data
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return View(dto);
        }

    }
}
