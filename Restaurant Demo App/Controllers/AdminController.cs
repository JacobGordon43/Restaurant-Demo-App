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
using Microsoft.AspNetCore.Authentication;
namespace Restaurant_Demo_App.Controllers
{
    [Route("/Admin")]
    public class AdminController : Controller
    {
        private static MenuService menuService = new MenuService();
        private static AboutService aboutService = new AboutService();
        private static AdminService adminService = new AdminService();
        private static InfoService infoService = new InfoService();
        private static SlideshowService slideshowService = new SlideshowService();
        private static List<MenuCategoryModel> Menu;
        private static AboutContentModel AboutContent;
        private static List<ImageModel> Slideshow;
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
/*        [Route("Logout")]
        public ActionResult LogoutAdmin()
        {
            _httpContextAccessor.HttpContext.User = UserService.User;

            foreach (ClaimsIdentity user in _httpContextAccessor.HttpContext.User.Identities)
            {
                if (user.IsAuthenticated)
                {
                    user.RemoveClaim(IsAuthenticated);
                }
            }
            return RedirectToAction("Index", "Home");
        }*/
        [HttpGet("Dashboard")]
        public ActionResult Index()
        {
            if (!adminUser.Authenticated){
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            InfoModel info = infoService.GetInfo();
            return View(info);
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

        [HttpGet("Menu")]
        public ActionResult MenuContentView()
        {
            Menu = menuService.GetMenu();
            return View("Menu", Menu);
        }

        [HttpGet("Slideshow")]
        public ActionResult SlideshowContentView()
        {
            Slideshow = slideshowService.GetSlideshow();
            return View("Slideshow", Slideshow);
        }
        [HttpGet("CreateImage")]
        public ActionResult CreateImageView()
        {
            return View("CreateImage");
        }
        [Route("InsertImage")]
        public IActionResult CreateImage(ImageModel image)
        {
            if (!adminUser.Authenticated)
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (slideshowService.AddToSlideshow(image))
            {
                TempData["SlideshowSuccess"] = "The Slideshow has been saved successfully!";
                TempData["SlideshowFailure"] = null;
            }
            else
            {
                TempData["SlideshowFailure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["SlideshowSuccess"] = null;
            }
            return RedirectToAction("SlideshowContentView");
        }
        [Route("DeleteImage")]
        public IActionResult DeleteImage(ImageModel image)
        {
            if (!adminUser.Authenticated)
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (slideshowService.DeleteImage(image))
            {
                TempData["SlideshowSuccess"] = "The Slideshow has been saved successfully!";
                TempData["SlideshowFailure"] = null;
            }
            else
            {
                TempData["SlideshowFailure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["SlideshowSuccess"] = null;
            }
            return RedirectToAction("SlideshowContentView");
        }
        [Route("UpdateAbout")]
        public IActionResult UpdateAbout(AboutContentModel aboutContent)
        {
            if (!adminUser.Authenticated)
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
                TempData["AboutFailure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["AboutSuccess"] = null;
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
            return RedirectToAction("MenuContentView");
        }

        [Route("CreateCategory")]
        public IActionResult CreateCategory([FromBody] MenuCategoryModel category)
        {
            if (!adminUser.Authenticated)
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (menuService.CreateCategory(category))
            {
                TempData["MenuSuccess"] = "The Menu has been saved successfully!";
                TempData["MenuFailure"] = null;
            }
            else
            {
                TempData["MenuFailure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["MenuSuccess"] = null;
            }
            return RedirectToAction("MenuContentView");
        }
        [Route("UpdateSlideshow")]
        public IActionResult UpdateSlideshow(ImageModel image)
        {
            if (!adminUser.Authenticated)
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (slideshowService.UpdateImage(image))
            {
                TempData["SlideshowSuccess"] = "The Slideshow has been saved successfully!";
                TempData["SlideshowFailure"] = null;
            }
            else
            {
                TempData["SlideshowFailure"] = "There has been an issue with updating the menu. You may need to contact the Developer to resolve this issue.";
                TempData["SlideshowSuccess"] = null;
            }
            return RedirectToAction("SlideshowContentView");
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
        [HttpGet("CreateDish")]

        public IActionResult CreateDishView()
        {
            if (!adminUser.Authenticated)
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            MenuItemCreateViewModel model = new MenuItemCreateViewModel();
            return View("CreateDish", model);
        }
        [Route("InsertDish")]
        public IActionResult CreateDish(MenuItemModel item)
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

        public IActionResult CreateHours(DayOpenModel hours)
        {
            if (!adminUser.Authenticated)
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (infoService.AddHours(hours))
            {
                TempData["HoursSuccess"] = "The hours has been saved successfully!";
                TempData["HoursFailure"] = null;
            }
            else
            {
                TempData["HoursFailure"] = "There has been an issue with updating the hours. You may need to contact the Developer to resolve this issue.";
                TempData["HoursSuccess"] = null;
            }
            return RedirectToAction("Index");
        }
        [Route("UpdateHours")]
        public IActionResult UpdateHours(DayOpenModel hours)
        {
            if (!adminUser.Authenticated)
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }
            if (infoService.UpdateHours(hours))
            {
                TempData["HoursSuccess"] = "The hours has been saved successfully!";
                TempData["HoursFailure"] = null;
            }
            else
            {
                TempData["HoursFailure"] = "There has been an issue with updating the hours. You may need to contact the Developer to resolve this issue.";
                TempData["HoursSuccess"] = null;
            }
            return RedirectToAction("Index");
        }
        [HttpGet("UpdateInfo")]
        public IActionResult UpdateInfo(InfoModel info)
        {
            if (!adminUser.Authenticated)
            {
                TempData["LoginFailure"] = "You must login to view that Admin Content";
                return RedirectToAction("Login");
            }

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
