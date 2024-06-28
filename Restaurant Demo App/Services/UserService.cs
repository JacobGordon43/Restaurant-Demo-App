using System.Security.Claims;

namespace Restaurant_Demo_App.Services
{
    public class UserService
    {
        public static ClaimsPrincipal User { get; set; }
        public UserService() { }

        public void SetUser(IHttpContextAccessor httpContext)
        {
            User = httpContext.HttpContext.User;
        }
    }
}
