using System.Security.Claims;

namespace ComicsAPI_V2.Helpers
{
    public class JwtReceiver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtReceiver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //make a method that gets all the claims from the JWT token

        public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
        public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public string? Role => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

        public string? Token => _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        public string? Username => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value
                                  ?? _httpContextAccessor.HttpContext?.User.FindFirst("unique_name")?.Value;

        /*
        var user = _httpContextAccessor.HttpContext?.User;
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var role = user?.FindFirst(ClaimTypes.Role)?.Value;
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        var username = user?.FindFirst(ClaimTypes.Name)?.Value
                                  ?? user?.FindFirst("unique_name")?.Value;*/
    }
}
