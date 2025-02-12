using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models.DTO;
using AustraliaSays2_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AustraliaSays2.Areas.Admin.Contollers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class SiteTypeController : Controller
    {

        private readonly ISiteTypeRepository _siteTypeRepository;
        public SiteTypeController(ISiteTypeRepository siteTypeRepository)
        {
            _siteTypeRepository = siteTypeRepository;
        }
        public async Task<IActionResult> Index()
        {
            //  var sitetypelist = await _siteTypeRepository.GetAll();
            return View(/*nameof(Index)*/);
        }


        public async Task<IActionResult> Insert(SiteTypeDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _siteTypeRepository.AddSiteTypesync(dto);
                return RedirectToAction("Index");
            }
            return View(dto);
        }

       
        public async Task<IActionResult> GetAll()
        {
            var sitetypelist = await _siteTypeRepository.GetAll();
            return Json(new { data = sitetypelist });
        }


        public async Task<IActionResult> Update(SiteTypeDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _siteTypeRepository.UpdateSiteTypesync(dto);
                return RedirectToAction("Index");
            }
            return View(dto);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
              var result= await _siteTypeRepository.DeleteSiteTypesync(id);
                if(result !=null)
                {
                    return Json(new { success=true, message="SiteType Deleted Successfully", id});
                }
                else
                {
                    return Json(new {success=false, message= "Failed to delete SiteType", id});
                }
            }
            return Json(new {success=false, message ="Model is not valid ", id });
        }
    }
}
