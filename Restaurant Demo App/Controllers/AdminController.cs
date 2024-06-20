using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Restaurant_Demo_App.Models;
using System.Diagnostics;
using System.IO;
using Humanizer.Localisation.DateToOrdinalWords;
using Restaurant_Demo_App.Services;
namespace Restaurant_Demo_App.Controllers
{
    [Route("/SecretPath")]
    public class AdminController : Controller
    {
        private MenuService menuService = new MenuService();
        private static List<MenuCategoryModel> Menu;


            // GET: AdminController
            [HttpGet("Dashboard")]
        public ActionResult Index()
        {
            Menu = menuService.GetMenu();
            
            List<AboutContentModel> AboutContent = new List<AboutContentModel>();

            AdminPageModel adminModel = new AdminPageModel(Menu, AboutContent);
            return View(adminModel);
        }

        [HttpGet("About")]
        public ActionResult AboutContent()
        {
            List<AboutContentModel> AboutContent = new List<AboutContentModel>();
            AboutContent.Add(new AboutContentModel("<p>Hello</p>"));
            return View(AboutContent);
        }

        [Route("Update")]
        public IActionResult UpdateMenu(MenuItemModel item)
        {
            if (menuService.UpdateMenu(item))
            {
                TempData["Success"] = "The Menu has been saved successfully!";
                TempData["Failure"] = null;
            }
            else
            {
                TempData["Failure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["Success"] = null;
            }
            return RedirectToAction("Index");
        }

        [Route("Delete")]
        public IActionResult DeleteItem(MenuItemModel item)
        {
            if (menuService.DeleteItem(item))
            {
                TempData["Success"] = "The Menu has been saved successfully!";
                TempData["Failure"] = null;
            }
            else
            {
                TempData["Failure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["Success"] = null;
            }

            return RedirectToAction("Index");
        }
        [HttpGet("Create")]

        public IActionResult CreateView()
        {
            MenuItemCreateViewModel model = new MenuItemCreateViewModel();
            return View("Create", model);
        }
        public IActionResult CreateItem(MenuItemModel item)
        {
            if (menuService.CreateItem(item))
            {
                TempData["Success"] = "The Menu has been saved successfully!";
                TempData["Failure"] = null;
            }
            else
            {
                TempData["Failure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["Success"] = null;
            }
            return RedirectToAction("Index");
        }
    }
}
