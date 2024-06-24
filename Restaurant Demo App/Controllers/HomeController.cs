using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restaurant_Demo_App.Models;
using Restaurant_Demo_App.Services;
using System.Diagnostics;

namespace Restaurant_Demo_App.Controllers
{
    public class HomeController : Controller
    {
        private MenuService menuService = new MenuService();
        private AboutService aboutService = new AboutService();
        private static List<MenuCategoryModel> Menu;
        private static AboutContentModel AboutContent;

        public IActionResult Index()
        {
           List<ImageModel> images = new List<ImageModel>();
           List<MenuCategoryModel> categories = new List<MenuCategoryModel>();
            try
            {
                using (StreamReader r = new StreamReader("wwwroot/json/slideshow_images.json"))
                {
                    string imagesString = r.ReadToEnd();
                    JObject imagesJson = JObject.Parse(imagesString);
                    Debug.WriteLine(imagesJson.ToString());
                    foreach (JObject imageJson in imagesJson.Values()) 
                    {
                        ImageModel image = JsonConvert.DeserializeObject<ImageModel>(imageJson.ToString());
                    images.Add(image);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            Menu = menuService.GetMenu();
            HomePageModel lists = new HomePageModel(Menu, images);
            return View(lists);
        }
        [HttpGet("About")]
        public IActionResult About()
        {
            AboutContent = aboutService.GetAboutContent();
            return View(AboutContent);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
