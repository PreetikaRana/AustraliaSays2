using AustraliaSays2_DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View(); 
        }
    }
}
