using AustraliaSays2_DataAccess.Repository;
using AustraliaSays2_DataAccess.Repository.IRepository;
using AustraliaSays2_Models;
using AustraliaSays2_Models.DTO;
using AustraliaSays2_Models.Models;
using AustraliaSays2_Models.ViewModels;
using AustraliaSays2_Utility;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.Win32.SafeHandles;
using System.Security.Claims;
using System.Security.Policy;

namespace AustraliaSays2.Areas.Admin.Contollers
{
    [Area("Admin")]

    public class ArticlesController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleTypeRepository _articleTypeRepository;
        private readonly ISiteRepository _siteRepository;
        public ArticlesController(IArticleRepository articleRepository, IArticleTypeRepository articleTypeRepository, ISiteRepository siteRepository)
        {
            _articleRepository = articleRepository;
            _articleTypeRepository = articleTypeRepository;
            _siteRepository = siteRepository;
        }

        #region Article Index Page
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 3)
        {
            var paginatedArticles = await _articleRepository.GetPaginatedArticlesAsync(pageIndex, pageSize);
            return View(paginatedArticles);
        }

        #endregion


        #region AddArticle View
        public async Task<IActionResult> AddArticle()
        {
            return View();
        }

        #endregion

        #region Get And Add Article
        [HttpGet]

        public async Task<IActionResult> Upsert()

        {
            ViewBag.ArticleTypes = await _articleTypeRepository.GetArticleTypeListAsync();
            ViewBag.Sites = await _siteRepository.GetSiteListAsync();
            return View(new ArticleDTO());

        }

        [HttpPost]
        //  [Authorize(Roles = SD.Role_Admin)]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ArticleDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //  dto.ApplicationUserId = _userManager.GetUserId(User);
                    var result = await _articleRepository.AddArticleAsync(dto, User);
                    if (result != null)
                    {
                        TempData["SuccessMessage"] = result != null ? "Article Added Successfully" : "Failed to Add Article";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Their is an issue while adding Article";
                    }
                    return RedirectToAction("Index"); // Redirect ensures Index view is loaded with proper data
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            ViewBag.ArticleTypes = await _articleTypeRepository.GetArticleTypeListAsync();
            ViewBag.Sites = await _siteRepository.GetSiteListAsync();
            return View(dto);

        }
        #endregion


        #region Comment Index List
        public async Task<IActionResult> Comment(int pageIndex = 1, int pageSize = 3)
        {
            var paginatedComments = await _articleRepository.GetPaginatedCommentsAsync(pageIndex, pageSize);
            var model = new CommentVM
            {
                Comments = paginatedComments.Items,
                ReportComment = new ReportComment(),
                TotalPages = paginatedComments.TotalPages,
                CurrentPage = pageIndex,
                HasPreviousPage = paginatedComments.HasPreviousPage,
                HasNextPage = paginatedComments.hasNextpage
            };

            return View(model);
        }

        #endregion

        #region Insrt Comment
        //    [Authorize(Roles = SD.Role_Commenter)]

        public async Task<IActionResult> InsertComment(CommentDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var comment = await _articleRepository.AddUpdateCommentAsync(dto, User);
                    if (dto.Id == 0)
                    {
                        TempData["SuccessMessage"] = comment != null ? "Comment Added Successfully" : "Failed To Add Comment";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Comment Updated Successfull";
                    }
                    return RedirectToAction("Comment");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            return View(dto);
        }

        #endregion

        #region Comment Index View

        public async Task<IActionResult> AddComment()
        {
            return View();
        }
        #endregion



        #region DeleteArticleBYId
        [HttpDelete]
        //  [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Commenter)]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _articleRepository.DeleteArticleAsync(id);

                if (result != null)
                {
                    return Json(new { success = true, message = "Article deleted successfully", id });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete Article", id });
                }
            }

            return Json(new { success = false, message = "Invalid model state", id });
        }
        #endregion

        #region Add Report
        [HttpPost]
        //     [Authorize(Roles = SD.Role_Reader)]
        public async Task<IActionResult> AddReport(ReportCommentDTO dto)
        {
            if (!Enum.IsDefined(typeof(ReportComment.Review), dto.ReportsComment))
            {
                TempData["ErrorMessage"] = "Invalid review.";
                return RedirectToAction("Comment");
            }
            try
            {

                var savedReview = await _articleRepository.AddReportAsync(dto, User);
                TempData["SuccessMessage"] = $"Review added successfully! Review ID: {savedReview.Id}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Comment");
        }
        #endregion

        #region Get All Article
        public async Task<IActionResult> GetAll()
        {
            var articlelist = await _articleRepository.GetAllArticlesAsync();
            return Json(new { data = articlelist });
        }
        #endregion

        #region Get and Update Article

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var article = await _articleRepository.Details(id);
            if (article == null)
            {
                return NotFound();
            }
            var articleDto = new ArticleDTO
            {
                Id = article.Id,
                ArticleTypeId = article.ArticleTypeId,
                Name = article.Name,
                Tags = article.Tags,
                SiteId = article.SiteId,
                URL = article.URL,
                Description = article.Description,
                ImagePath = article.ImagePath
            };
            ViewBag.ArticleTypes = await _articleTypeRepository.GetArticleTypeListAsync();
            ViewBag.Sites = await _siteRepository.GetSiteListAsync();
            return View(articleDto);

        }

        [HttpPost]
        public async Task<IActionResult> Update(ArticleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ArticleTypes = await _articleTypeRepository.GetArticleTypeListAsync();
                ViewBag.Sites = await _siteRepository.GetSiteListAsync();
                return View(dto);
            }

            try
            {
                //  dto.ApplicationUserId = _userManager.GetUserId(User);
                var result = await _articleRepository.UpdateArticleAsync(dto, User);
                if (result != null)
                {
                    TempData["SuccessMessage"] = result != null ? "Article Updated Successfully" : "Failed to Update Article";
                }
                else
                {
                    TempData["ErrorMessage"] = "Their is an issue while Updating Article";
                }
                return RedirectToAction("Index"); // Redirect ensures Index view is loaded with proper data
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                ViewBag.ArticleTypes = await _articleTypeRepository.GetArticleTypeListAsync();
                ViewBag.Sites = await _siteRepository.GetSiteListAsync();
                return View(dto);
            }


        }
        #endregion

    }
}
