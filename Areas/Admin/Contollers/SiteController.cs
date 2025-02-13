using AustraliaSays2_DataAccess.Repository;
using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models.DTO;
using AustraliaSays2_Models.Models;
using AustraliaSays2_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AustraliaSays2.Areas.Admin.Contollers
{
    [Area("Admin")]
  //  [Authorize]
   
    public class SiteController : Controller
    {
        private readonly ISiteRepository _siteRepository;
        private readonly ISiteTypeRepository _siteTypeRepository;
        public SiteController(ISiteRepository siteRepository, ISiteTypeRepository siteTypeRepository)
        {
                _siteRepository = siteRepository;
            _siteTypeRepository = siteTypeRepository;
        }
    

        public async Task<IActionResult> Index(int pageIndex=1, int pageSize=4)
        {
            var paginatesdites = await _siteRepository.GetAllPagesAsync(pageIndex, pageSize);
            return View(paginatesdites);
        }

        [HttpGet]
        
        public async Task<IActionResult> Insert()
        {
            ViewBag.Countries = await _siteRepository.GetCountryListAsync();
            ViewBag.SiteTypes = await _siteTypeRepository.GetSiteTypeListAsync();
            return View(new SiteDTO());
        }

        [HttpPost]

   
        public async Task<IActionResult> Insert(SiteDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _siteRepository.AddSiteAsync(dto);
                    if (result != null)
                    {
                        TempData["SuccessMessage"] =  "Site Added Successfully";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "THeir is an Issue While saving Site";
                    }
                    return RedirectToAction("Index"); // Redirect ensures Index view is loaded with proper data
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            ViewBag.Countries = await _siteRepository.GetCountryListAsync();
            ViewBag.SiteTypes = await _siteTypeRepository.GetSiteTypeListAsync();
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var site = await _siteRepository.Details(id);
            if (site == null)
            {
                return NotFound();
            }

            var siteDto = new SiteDTO
            {
                Id = site.Id,
                Name = site.Name,
                CountryId = site.CountryId,
                URL = site.URL,
                SiteTypeId = site.SiteTypeId,
                LogoPath = site.LogoPath // Pass existing logo path to DTO
            };
            ViewBag.Countries = await _siteRepository.GetCountryListAsync() ?? new List<SelectListItem>();
            ViewBag.SiteTypes = await _siteTypeRepository.GetSiteTypeListAsync() ?? new List<SelectListItem>();
            return View(siteDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SiteDTO siteDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = await _siteRepository.GetCountryListAsync() ?? new List<SelectListItem>();
                ViewBag.SiteTypes = await _siteTypeRepository.GetSiteTypeListAsync() ?? new List<SelectListItem>();
                return View(siteDto);
            }

            try
            {
                var updatedSite = await _siteRepository.UpdateSiteAsync(siteDto);
                return RedirectToAction("Index"); // Redirect after successful update
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Countries = await _siteRepository.GetCountryListAsync() ?? new List<SelectListItem>();
                ViewBag.SiteTypes = await _siteTypeRepository.GetSiteTypeListAsync() ?? new List<SelectListItem>();
                return View(siteDto);
            }
        }


        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> AddSite()
        {
            return View();
        }

        public async Task<IActionResult> GetAll()
        {
            var sites = await _siteRepository.GetAllSitesAsync();
            return Json(new { data = sites });
        }
        #region DeleteSiteBYId
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return Json(new { success = false, message = "Invalid Article ID" });
            }
            if (ModelState.IsValid)
            {
                var result = await _siteRepository.DeleteSiteAsync(id);

                if (result != null)
                {
                    return Json(new { success = true, message = "Site deleted successfully", id });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete Site", id });
                }
            }

            return Json(new { success = false, message = "Invalid model state", id });
        }
        #endregion



        #region

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var site = await _siteRepository.Details(id); // Replace with your data fetching logic
            if (site == null)
            {
                return NotFound();
            }

            return Json(site); // Return the data as JSON
        }
        #endregion
    }
}
