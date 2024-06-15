using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restaurant_Demo_App.Models;
using System.Diagnostics;

namespace Restaurant_Demo_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

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

            try
            {
                using (StreamReader r = new StreamReader("wwwroot/json/menu.json"))
                {
                    string menuCategories = r.ReadToEnd();
                    JObject menuCategoriesJson = JObject.Parse(menuCategories);
                    Debug.WriteLine(menuCategoriesJson.ToString());
                    foreach (JObject menuCategoryJson in menuCategoriesJson["Menu"].Values())
                    {
                        Debug.WriteLine(menuCategoriesJson.ToString());
                        Debug.WriteLine(menuCategoryJson.ToString());
                        MenuCategoryModel category = JsonConvert.DeserializeObject<MenuCategoryModel>(menuCategoryJson.ToString());
                        Debug.WriteLine("The category is " + category.Category);
                        foreach(MenuItemModel item in category.Items)
                        {
                            Debug.WriteLine(item.Price);
                        }
                        categories.Add(category);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            HomePageModel lists = new HomePageModel(categories, images);
            return View(lists);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
