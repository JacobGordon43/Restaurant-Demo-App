using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Restaurant_Demo_App.Models;
using System.Diagnostics;
using System.IO;
namespace Restaurant_Demo_App.Controllers
{
    [Route("/SecretPath")]
    public class AdminController : Controller
    {
        // GET: AdminController
        [HttpGet("Home")]
        public ActionResult Index()
        {
            List<MenuCategoryModel> Menu = new List<MenuCategoryModel>();
            List<AboutContentModel> AboutContent = new List<AboutContentModel>();
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
                        foreach (MenuItemModel item in category.Items)
                        {
                            Debug.WriteLine(item.Price);
                        }
                        Menu.Add(category);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            AdminPageModel adminModel = new AdminPageModel(Menu, AboutContent);
            return View(adminModel);
        }
        public ActionResult UpdateMenu(List<MenuCategoryModel> menu)
        {
            string rootDir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            Debug.WriteLine(rootDir);
            try
            {
                string menuCategoriesJson = JsonConvert.SerializeObject(menu);
                System.IO.File.WriteAllText(rootDir + "//wwwroot//json//menu.json", menuCategoriesJson);
                TempData["Success"] = "The Menu has been saved successfully!";
                TempData["Failure"] = null;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                TempData["Failure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["Success"] = null;
            }

            return RedirectToAction("Index");
        }
    }
}
