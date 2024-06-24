using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Restaurant_Demo_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPI : ControllerBase
    {
        [Authorize]
        [HttpGet("/")]
        public IActionResult GetData()
        {
            return Ok();
        }
    }
}
