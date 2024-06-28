using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Restaurant_Demo_App.Controllers
{
    [Route("/api")]
    [ApiController]
    public class AuthAPI : ControllerBase
    {
        [Authorize]
        [HttpGet("GetData")]
        public IActionResult GetData()
        {
            return Ok();
        }
    }
}
