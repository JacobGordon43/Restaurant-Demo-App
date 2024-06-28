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
using System.Security.Claims;
namespace Restaurant_Demo_App.Controllers
{
    [Route("/SecretPath/Admin")]
    public class AdminController : Controller
    {
        private MenuService menuService = new MenuService();
        private AboutService aboutService = new AboutService();
        private AdminService adminService = new AdminService();
        private static List<MenuCategoryModel> Menu;
        private static AboutContentModel AboutContent;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //Keeps the Admin persisting
        private static AdminModel adminUser = adminUser == null ? new AdminModel() : adminUser;
        public AdminController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("LoginAdmin")]
        public async Task<ActionResult> LoginAdmin(AdminModel admin)
        {
            if (await adminService.LoginAdmin(admin))
            {
                _httpContextAccessor.HttpContext.User = UserService.User;

                foreach (ClaimsIdentity user in _httpContextAccessor.HttpContext.User.Identities)
                {
                    if (user.IsAuthenticated)
                    {
                        adminUser = admin;
                        adminUser.Authenticated = true;
                        adminUser.Name = user.FindFirst(ClaimTypes.GivenName).Value;
                        adminUser.Id = user.FindFirst(ClaimTypes.Sid).Value;
                    }
                }

                return RedirectToAction("Index");
            }
            else
            {
                TempData["LoginFailure"] = "You were not successfully logged in";
                return View("Login");
            }
        }

        [HttpGet("Dashboard")]
        public ActionResult Index()
        {
            if (!adminUser.Authenticated){
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            Menu = menuService.GetMenu();
            
            List<AboutContentModel> AboutContent = new List<AboutContentModel>();

            AdminPageModel adminModel = new AdminPageModel(Menu, AboutContent);
            return View(adminModel);
        }

        [HttpGet("About")]
        public ActionResult AboutContentView()
        {
            if (!adminUser.Authenticated)
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            AboutContent = aboutService.GetAboutContent();
            return View("AboutContent", AboutContent);
        }

        [Route("UpdateAbout")]
        public IActionResult UpdateAbout(AboutContentModel aboutContent)
        {
            if (adminUser.Authenticated)
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (!aboutService.UpdateAboutContent(aboutContent))
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
            if (!adminUser.Authenticated)
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
            if (!adminUser.Authenticated)
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
        [HttpGet("Create")]

        public IActionResult CreateView()
        {
            if (!adminUser.Authenticated)
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            MenuItemCreateViewModel model = new MenuItemCreateViewModel();
            return View("Create", model);
        }
        public IActionResult CreateItem(MenuItemModel item)
        {
            if (!adminUser.Authenticated)
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
    }
}
