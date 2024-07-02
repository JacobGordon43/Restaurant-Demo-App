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
        private InfoService infoService = new InfoService();
        private SlideshowService slideshowService = new SlideshowService();
        private static List<MenuCategoryModel> Menu;
        private static AboutContentModel AboutContent;
        private static List<ImageModel> Images;
        public IActionResult Index()
        {
            InfoModel info = infoService.GetInfo();
            string rawPhoneNum = info.PhoneNumber.Replace("-", "");
            ViewData["phone-col"] = "<a href='tel:'+0" + rawPhoneNum + "'>" + info.PhoneNumber + "</a>";
            ViewData["address"] = info.Address;
            ViewData["business-hours"] = "";
            foreach(DayOpenModel businessHour in info.BusinessHours)
            {
                ViewData["business-hours"] += "<div class='hours'>" + businessHour.Day + ":" + (businessHour.IsClosed ? "Closed" : businessHour.OpenTime + " - " + businessHour.CloseTime + "</div> <br/>"); 
            }
            List<ImageModel> images = new List<ImageModel>();
            List<MenuCategoryModel> categories = new List<MenuCategoryModel>();

            Images = slideshowService.GetSlideshow();
            Menu = menuService.GetMenu();
            HomePageModel lists = new HomePageModel(Menu, Images);
            return View(lists);
        }
        [HttpGet("About")]
        public IActionResult About()
        {
            AboutContent = aboutService.GetAboutContent();
            return View(AboutContent);
        }

        public async Task<IActionResult> SendEmail(EmailModel email)
        {
            EmailService service = new EmailService();
            bool success = await service.SendEmail(email);

            if (success)
            {
                TempData["EmailSuccess"] = "The email was sent successfully";
            }
            else
            {
                TempData["EmailFailure"] = "There was an issue sending the email";
            }

            return RedirectToAction("Contact");
        }
        public IActionResult Contact()
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
