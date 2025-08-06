using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Models;

namespace ComicsAPI.DataAccess
{
    public class AuthenticationDbContext : IdentityDbContext
    {

        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var NormalUserRoleId = "08014d18-e4c5-4b44-9e1a-859626270bc4";
            var ModeratorRoleId = "e06d1c43-60d0-4d74-b69b-f0a15c6c636a";

            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = NormalUserRoleId,
                    ConcurrencyStamp = NormalUserRoleId,
                    Name = "NormalUser",
                    NormalizedName = "NormalUser".ToUpper()
                },
                new IdentityRole()
                {
                    Id = ModeratorRoleId,
                    ConcurrencyStamp = ModeratorRoleId,
                    Name = "Moderator",
                    NormalizedName = "Moderator".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }


    }
}
