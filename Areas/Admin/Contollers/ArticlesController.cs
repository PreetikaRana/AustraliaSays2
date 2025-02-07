using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models;
using AustraliaSays2_Models.DTO;
using AustraliaSays2_Models.Models;
using AustraliaSays2_Models.ViewModels;
using AustraliaSays2_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.Win32.SafeHandles;

namespace AustraliaSays2.Areas.Admin.Contollers
{
    [Area("Admin")]
   
    public class ArticlesController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        public ArticlesController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        public async Task<IActionResult> Index()
        {
            var articles = await _articleRepository.GetAllArticlesAsync();
            if (articles == null || !articles.Any()) 
            {
                articles = new List<Article>(); 
            }
            return View(articles);
        }
      //  [Authorize(Roles = SD.Role_Admin)]
     //   [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ArticleDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _articleRepository.AddUpdateArticleAsync(dto);
                    if (dto.Id == null || dto.Id == 0)
                    {
                        TempData["SuccessMessage"] = result != null ? "Article Added Successfully" : "Failed to Add Article";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Article Updated Successfully";
                    }
                    return RedirectToAction("Index"); // Redirect ensures Index view is loaded with proper data
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return View(dto);
            //    if (ModelState.IsValid)
            //    {
            //        try
            //        {
            //            var result = await _articleRepository.AddUpdateArticleAsync(dto);
            //            if (dto.Id == null || dto.Id == 0)
            //            {
            //                try
            //                {
            //                    if (result != null) 
            //                    {
            //                        TempData["SuccessMessage"] = "Article Added Successfully";                               
            //                    }

            //                }
            //                catch(Exception ex)
            //                {
            //                    TempData["ErrorMessage"] = ex.Message;
            //                }                       
            //            }
            //            else
            //            {
            //                TempData["SuccessMessage"] = "Article Updated Successfully";
            //            }
            //   //         var articleslist = await _articleRepository.GetAllArticlesAsync();  

            //            return RedirectToAction("Index");
            //        }
            //        catch (Exception ex)
            //        {
            //            TempData["ErrorMessage"] = ex.Message;
            //        }

            //    }
            //    return View(dto);
            //}
        }
     //   [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Commenter)]
     //   [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment()
        {
            var comments = await _articleRepository.GetAllCommentsAsync();
            var models = new CommentVM
            {
                Comments = comments,
                ReportComment = new ReportComment()
            };
            return View(models);

        }
       // [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Commenter)]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertComment(CommentDTO dto)
        {
           if(ModelState.IsValid)
            {
                try
                {
                    var comment = await _articleRepository.AddUpdateCommentAsync(dto);
                    if(dto.Id == 0 )
                    {
                        TempData["SuccessMessage"] = comment != null ? "Comment Added Successfully" : "Failed To Add Comment";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Comment Updated Successfull";
                    }
                    return RedirectToAction("Comment");
                }
                catch(Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return View(dto);
        }
      //  [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Commenter)]
      //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0 || id == null)
                    {
                        return BadRequest("Invalid article ID.");
                    }


                    bool isDeleted = await _articleRepository.DeleteArticleAsync(id);

                    if (!isDeleted)
                    {
                        return NotFound("Article not found or already deleted.");
                    }

                    TempData["SuccessMessage"] = "Article deleted successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction("Index");
                }
            }
            return View(id);
        }

        [HttpPost]
        public async Task<IActionResult> AddReport(ReportCommentDTO dto)
        {
            if (!Enum.IsDefined(typeof(ReportComment.Review), dto.ReportsComment))
            {
                TempData["ErrorMessage"] = "Invalid review.";
                return RedirectToAction("Comment");
            }
            try
            {
               
                var savedReview = await _articleRepository.AddReportAsync(dto);
                TempData["SuccessMessage"] = $"Review added successfully! Review ID: {savedReview.Id}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Comment");
        }


        //public async Task<IActionResult> LoadComment()
        //{
        //    var comments = await _articleRepository.GetAllCommentsAsync();
        //    var models = new CommentVM
        //    {
        //        Comments = comments,
        //        ReportComment = new ReportComment()
        //    };
        //    return View(models);
        //}
    }
}
