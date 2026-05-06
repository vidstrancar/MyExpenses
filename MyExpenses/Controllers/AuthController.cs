using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyExpenses.Dtos;
using MyExpenses.Entities;

namespace MyExpenses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        private static User User = new();
        
        [HttpPost("register")]
        public ActionResult<User> Register(UserDto userDto)
        {
            var hashedPassword = new PasswordHasher<User>().HashPassword(User, userDto.Password);
            
            User.Username = userDto.Username;
            User.PasswordHash = hashedPassword;
            
            return Ok(User);
        }

        [HttpPost("login")]
        public ActionResult<string> Login(UserDto userDto)
        {
            if (userDto.Username != User.Username)
            {
                return BadRequest("User not found");
            }
            
            var passwordVerification = new PasswordHasher<User>()
                .VerifyHashedPassword(User, User.PasswordHash, userDto.Password);

            if (passwordVerification == PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid password");
            }

            string token = CreateToken(User);
            
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username)
            };
            
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
