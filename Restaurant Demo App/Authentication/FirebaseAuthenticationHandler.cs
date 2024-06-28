using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Restaurant_Demo_App.Services;
namespace Restaurant_Demo_App.Authentication
{

    public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly FirebaseApp _firebaseApp;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserService userService = new UserService();
        public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, FirebaseApp firebaseApp, IHttpContextAccessor httpContextAccessor) : base(options, logger, encoder)
        {
            _firebaseApp = firebaseApp;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Context.Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.NoResult();
            }

            string bearerToken = Context.Request.Headers["Authorization"];

            if(bearerToken != null && !bearerToken.StartsWith("Bearer"))
            {
                return AuthenticateResult.Fail("Invalid scheme");
            }
            try
            {
                string token = bearerToken.Substring("Bearer ".Length);
                FirebaseToken firebaseToken = await FirebaseAuth.GetAuth(_firebaseApp).VerifyIdTokenAsync(token);
                _httpContextAccessor.HttpContext.User.AddIdentity(new ClaimsIdentity(ToClaims(firebaseToken.Claims), nameof(FirebaseAuthenticationHandler)));
                userService.SetUser(_httpContextAccessor);
                return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new List<ClaimsIdentity>()
                {
                    new ClaimsIdentity(ToClaims(firebaseToken.Claims), nameof(FirebaseAuthenticationHandler))
                }), JwtBearerDefaults.AuthenticationScheme));
            }catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }

        }

        private IEnumerable<Claim> ToClaims(IReadOnlyDictionary<string, object> claims)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Sid, claims["user_id"].ToString()),
                new Claim(ClaimTypes.Email, claims["email"].ToString()),
                new Claim(ClaimTypes.GivenName, claims["name"].ToString())
            };
        }
    }
}
