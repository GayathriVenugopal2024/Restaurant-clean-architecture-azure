using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Restaurants.Application.Auth.Login;
using Restaurants.Application.Auth.Register;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthController(UserManager<IdentityUser> _userManager,IConfiguration configuration)
        {
            userManager = _userManager;
            this.configuration = configuration;
        }

        public UserManager<IdentityUser> userManager { get; }

        [HttpPost]
        [Route("Register")]
       public async Task <IActionResult> RegisterUser([FromBody] RegisterRequestDto registerRequestDto )
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName

            };
           var identityResult= await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if(identityResult.Succeeded)
            {
                if(registerRequestDto.Roles !=null && registerRequestDto.Roles.Any())
                {
                    identityResult= await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User registered! Please Login.");
                    }
                }
                
            }
            return BadRequest("Something Went wrong");
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user=await userManager.FindByEmailAsync(loginRequestDto.UserName);
            if(user !=null)
            {
               var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if(checkPasswordResult)
                {
                    var roles=await userManager.GetRolesAsync(user);
                    if(roles .Any())
                    {
                        //Create Token
                      var jwtToken = CreateJWTToken(user, roles.ToList());
                        LoginResponseDto response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                    
                    return Ok();
                }
            }
            return BadRequest("Username or Password incorrect");
        }
        private string CreateJWTToken(IdentityUser user,List<string> roles)
        {
            //Create claim
            var claims =new  List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credetial=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var toke = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires:DateTime.Now.AddMinutes(15),
                signingCredentials: credetial
                );
        return new JwtSecurityTokenHandler().WriteToken(toke);
        }
    }
    
}
