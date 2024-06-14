using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Demo_App.Models;

namespace Restaurant_Demo_App.Controllers
{
    [Route("/SecretPath")]
    public class AdminController : Controller
    {
        // GET: AdminController
        [HttpGet("Home")]
        public ActionResult Index()
        {
            List<MenuModel> list = new List<MenuModel>();
            AdminPageModel adminModel = new AdminPageModel();
            return View();
        }
    }
}
