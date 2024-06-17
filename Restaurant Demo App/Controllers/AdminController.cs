using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Restaurant_Demo_App.Models;
using System.Diagnostics;
using System.IO;
using Humanizer.Localisation.DateToOrdinalWords;
namespace Restaurant_Demo_App.Controllers
{
    [Route("/SecretPath")]
    public class AdminController : Controller
    {
        private static List<MenuCategoryModel> Menu;


            // GET: AdminController
            [HttpGet("Dashboard")]
        public ActionResult Index()
        {

            List<MenuCategoryModel> categories = new List<MenuCategoryModel>();
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
                        categories.Add(category);
                        Menu = categories;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            
            List<AboutContentModel> AboutContent = new List<AboutContentModel>();

            AdminPageModel adminModel = new AdminPageModel(categories, AboutContent);
            return View(adminModel);
        }
        public IActionResult UpdateMenu(MenuItemModel item)
        {
            string rootDir = @Directory.GetCurrentDirectory().ToString();
            string filePath = Path.Combine(rootDir, "/root/json/menu.json");
            Debug.WriteLine(rootDir);
            try
            {
                foreach(MenuCategoryModel category in Menu)
                {
                    if (category.Category == item.Category)
                    {
                        category.Items.ForEach(dish =>
                        {
                            if (dish.Id == item.Id)
                            {
                                dish = item;
                            }
                        });
                    }
                }
                string menuCategoriesJson = "";
                int count = 0;
                foreach (MenuCategoryModel category in Menu)
                {

                    string json = JsonConvert.SerializeObject(category);
                    menuCategoriesJson += category.Category + ":" + json;
                    if(count < Menu.Count() - 1)
                    {
                        menuCategoriesJson += ",";
                    }
                    count++;
                }
                
                System.IO.File.WriteAllText(rootDir + "/wwwroot/json/menu.json", "{'Menu':{" + menuCategoriesJson + "}}");
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
