using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UsersApi.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /*
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            //Create claims
            var claims = new List<Claim>()
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                 new Claim(ClaimTypes.Name, user.UserName), // 👈 username in token
                
            };
          //  claims.Add(new Claim(ClaimTypes.Email, user.Email));
            
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(1500),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/

        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>()
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                 new Claim(ClaimTypes.Name, user.UserName), // 👈 username in token
                 new Claim(JwtRegisteredClaimNames.Iss, "https://localhost:7196/"),
                  new Claim(JwtRegisteredClaimNames.Aud, "https://localhost:7196/"), // UsersAPI
                  new Claim(JwtRegisteredClaimNames.Aud, "https://localhost:7082/"),  // ComicsAPI
                  new Claim(JwtRegisteredClaimNames.Aud, "https://localhost:7194/")  // IssuesAPI
                
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],       // Identity API (7196)
                audience: configuration["Jwt:Audience"],        // Comics API audience
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),   // 30 minute expiration
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
