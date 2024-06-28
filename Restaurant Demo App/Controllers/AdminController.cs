using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Restaurant_Demo_App.Models;
using System.Diagnostics;
using System.IO;
using Humanizer.Localisation.DateToOrdinalWords;
using Restaurant_Demo_App.Services;
using Microsoft.AspNetCore.Identity;
namespace Restaurant_Demo_App.Controllers
{
    [Route("/SecretPath/Admin")]
    public class AdminController : Controller
    {
        private MenuService menuService = new MenuService();
        private AboutService aboutService = new AboutService();
        private AdminService adminService = new AdminService();
        private InfoService infoService = new InfoService();
        private static List<MenuCategoryModel> Menu;
        private static AboutContentModel AboutContent;

        [HttpGet("Login")]
        public ActionResult Login()
        {
            return View();
        }
        [Route("LoginAdmin")]
        public async Task<ActionResult> LoginAdmin(AdminModel admin)
        {
            if(await adminService.LoginAdmin(admin)){
                HttpContext.Session.SetString("Name", admin.Name);
                HttpContext.Session.SetString("Id", admin.Id);
                return View("Index");
            }
            else
            {
                TempData["LoginFailure"] = "You were not successfully logged in";
                return View("Login");
            }
        }

        [HttpGet("Signup")]
        public ActionResult Signup()
        {
            return View();
        }

        [Route("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin(AdminModel admin)
        {
            if (await adminService.CreateAdmin(admin))
            {
                return RedirectToAction("Login");
            }
            else
            {
                TempData["SignupFailure"] = "Your account was unable to be created. This might've been an issue with the server or there is already an admin account made. Please contact your developer if this problem persists.";
                return RedirectToAction("Signup");
            }
        }
            // GET: AdminController
        [HttpGet("Dashboard")]
        public ActionResult Index()
        {
            /*            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Id"))){
                            TempData["LoginFailure"] = "You must login to view that Admin Content";
                            return RedirectToAction("Login");
                        }*/

            InfoModel info = new InfoModel();
            return View(info);
        }

        [HttpGet("About")]
        public ActionResult AboutContentView()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Id")))
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            AboutContent = aboutService.GetAboutContent();
            return View("AboutContent", AboutContent);
        }

        [HttpGet("Menu")]
        public ActionResult MenuContentView()
        {
            Menu = menuService.GetMenu();
            return View("Menu", Menu);
        }

        [Route("UpdateAbout")]
        public IActionResult UpdateAbout(AboutContentModel aboutContent)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Id")))
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (aboutService.UpdateAboutContent(aboutContent))
            {
                TempData["AboutSuccess"] = "The Menu has been saved successfully!";
                TempData["Aboutailure"] = null;
            }
            else
            {
                TempData["MenuFailure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["MenuSuccess"] = null;
            }
            return RedirectToAction("AboutContentView");
        }
        [Route("UpdateMenu")]
        public IActionResult UpdateMenu(MenuItemModel item)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Id")))
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (menuService.UpdateMenu(item))
            {
                TempData["MenuSuccess"] = "The Menu has been saved successfully!";
                TempData["MenuFailure"] = null;
            }
            else
            {
                TempData["MenuFailure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["MenuSuccess"] = null;
            }
            return RedirectToAction("Index");
        }

        [Route("Delete")]
        public IActionResult DeleteItem(MenuItemModel item)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Id")))
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (menuService.DeleteItem(item))
            {
                TempData["MenuSuccess"] = "The Menu has been saved successfully!";
                TempData["MenuFailure"] = null;
            }
            else
            {
                TempData["MenuFailure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["MenuSuccess"] = null;
            }

            return RedirectToAction("Index");
        }
        [HttpGet("CreateDish")]

        public IActionResult CreateDishView()
        {
/*            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Id")))
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }*/
            MenuItemCreateViewModel model = new MenuItemCreateViewModel();
            return View("CreateDish", model);
        }
        [Route("InsertDish")]
        public IActionResult CreateDish(MenuItemModel item)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Id")))
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (menuService.CreateItem(item))
            {
                TempData["MenuSuccess"] = "The Menu has been saved successfully!";
                TempData["MenuFailure"] = null;
            }
            else
            {
                TempData["MenuFailure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["MenuSuccess"] = null;
            }
            return RedirectToAction("Index");
        }
        [HttpGet("CreateHours")]
        public IActionResult CreateHoursView()
        {
            /*            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Id")))
                        {
                            TempData["LoginFailure"] = "You must login to view that Admin Content";
                            return RedirectToAction("Login");
                        }*/
            return View("CreateHours");
        }
        [Route("InsertHours")]
        public IActionResult CreateHours(MenuItemModel item)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Id")))
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (menuService.CreateItem(item))
            {
                TempData["MenuSuccess"] = "The Menu has been saved successfully!";
                TempData["MenuFailure"] = null;
            }
            else
            {
                TempData["MenuFailure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["MenuSuccess"] = null;
            }
            return RedirectToAction("Index");
        }

        public IActionResult UpdateInfo(InfoModel info)
        {
            if (infoService.UpdateInfo(info))
            {
                TempData["InfoSuccess"] = "The info has been saved successfully!";
                TempData["InfoFailure"] = null;
            }
            else
            {
                TempData["InfoFailure"] = "There has been an issue with updating the info. You may need to contact the Developer to resolve this issue.";
                TempData["InfoSuccess"] = null;
            }
            return RedirectToAction("Index");
        }
    }
}
