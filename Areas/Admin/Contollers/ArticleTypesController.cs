using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AustraliaSays2.Areas.Admin.Contollers
{
    [Area("Admin")]
    public class ArticleTypesController : Controller
    {

        private readonly IArticleTypeRepository _articleTypeRepository;
        public ArticleTypesController(IArticleTypeRepository articleTypeRepository)
        {
            _articleTypeRepository = articleTypeRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region Get ArticleList
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articletypelist = await _articleTypeRepository.GetAll();
            return Json(new { data = articletypelist });
        }
        #endregion
        public async Task<IActionResult> Insert(ArticleTypeDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _articleTypeRepository.AddArticleTypeAsync(dto);
                return RedirectToAction("Index");
            }
            return View(dto);
        }
        #region DeleteTypeBYId
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _articleTypeRepository.DeleteArticleTypeAsync(id);

                if (result !=null)
                {
                    return Json(new { success = true, message = "Article type deleted successfully", id });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete article type", id });
                }
            }

            return Json(new { success = false, message = "Invalid model state", id });
        }
        #endregion

        public async Task<IActionResult> Update(ArticleTypeDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _articleTypeRepository.UpdateArticleTypeAsync(dto);
                return RedirectToAction("Index");
            }
            return View(dto);
        }
    }
}

