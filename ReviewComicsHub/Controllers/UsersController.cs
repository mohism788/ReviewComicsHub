using ComicsAPI.DTOs.UserDTOs;
using ComicsAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsersAPI.DTOs;
using UsersAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITokenRepository tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        this.userManager = userManager;
        this.tokenRepository = tokenRepository;
    }

    [HttpPost]
    [Route("Register")]

    public async Task<IActionResult> Register([FromBody] RegisterDto registerRequestDto)
    {
        var IdentityUser = new IdentityUser()
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };
        var IdentityResult = await userManager.CreateAsync(IdentityUser, registerRequestDto.Password);

        if (IdentityResult.Succeeded)
        {
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                IdentityResult = await userManager.AddToRolesAsync(IdentityUser, registerRequestDto.Roles);

                if (IdentityResult.Succeeded)
                {
                    return Ok("User registered successfully");
                }
            }
        }
        return BadRequest("Something went wrong");
    }


    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto loginRequestDto)
    {
        var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

        if (user != null)
        {
            var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (checkPasswordResult)
            {

                //Get roles for this user 
                var roles = await userManager.GetRolesAsync(user);
                if (roles != null)
                {
                    var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                    var response = new LoginResponseDto()
                    {
                        JwtToken = jwtToken,
                    };
                    return Ok(response);
                }
            }
        }

        return BadRequest("Username or password is incorrect!");
    }
}

