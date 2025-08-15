using Microsoft.AspNetCore.Identity;

namespace UsersApi.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
