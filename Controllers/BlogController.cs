using BlogApp.ElasticSearch.WEB.Interfaces;
using BlogApp.ElasticSearch.WEB.Models;
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

        [HttpGet]
        public async Task<IActionResult> Search()
        {
            return View(new List<Blog>(await _blogService.SearchAsync(string.Empty))); 
            // Sayfa ilk açıldığında tüm dataları göstermek için bunu yaptık.
            // Empty yani boş data  ile service ordan da repositorye gittiğimizde repository tarafı boş ya da null
            // ise tüm datayı gösteriyor şeklinde dinamik şekilde yapmıştık çünkü.
            // Yani kısaca sayfa ilk açıldığında kullanıcıya tüm datayı dönmek için yaptık
        }

        [HttpPost]
        public async Task<IActionResult> Search( string searchText)
        {

            ViewBag.searchText = searchText; // UI tarafında arayacağımız şeyi yazıp aradıktan sonra sonuç geldiğinde aradığımız kelime input alannından kayboluyordu kaybolmaması için ViewBag'e aldık.
             var blogList = await _blogService.SearchAsync(searchText);

            return View(blogList);
        }


    }
}
