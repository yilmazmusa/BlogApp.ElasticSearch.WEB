using BlogApp.ElasticSearch.WEB.Interfaces;
using BlogApp.ElasticSearch.WEB.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.ElasticSearch.WEB.Controllers
{
    public class BlogController : Controller
    {
        private IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public IActionResult Save()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateViewModel model)
        {

            var response = await _blogService.SaveAsync(model);

            if (!response)
            {
                TempData["result"] = "Kayıt Başarısız!!!";
                return RedirectToAction(nameof(BlogController.Save)); // Tip güvenli bir şekilde yukardaki(17.satırdaki) Save metoduna yönlendirdil.
            }

            TempData["result"] = "Kayıt Başarılı.";
            return RedirectToAction(nameof(BlogController.Save));
        }


    }
}
