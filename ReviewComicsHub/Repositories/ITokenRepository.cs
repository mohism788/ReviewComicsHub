using Microsoft.AspNetCore.Identity;

namespace ComicsAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
