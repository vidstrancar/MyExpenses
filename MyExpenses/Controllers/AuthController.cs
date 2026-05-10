using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyExpenses.Dtos;
using MyExpenses.Entities;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(UserRequestDto userRequestDto)
        {
            var user = await authService.RegisterUserAsync(userRequestDto);

            if (user == null)
            {
                return BadRequest("Username already exists");
            }
            
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserRequestDto userRequestDto)
        {
            var user = await authService.LoginUserAsync(userRequestDto);

            if (user == null)
            {
                return BadRequest("Username or password is incorrect");
            }
            
            return Ok(user);
        }
        
        [Authorize]
        [HttpGet("authTest")]
        public ActionResult AuthenticatedOnly()
        {
            return Ok("This is an authenticated only endpoint");
        }
    }
}
