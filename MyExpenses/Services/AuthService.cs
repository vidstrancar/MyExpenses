using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyExpenses.Data;
using MyExpenses.Dtos;
using MyExpenses.Entities;

namespace MyExpenses.Services;

public class AuthService(AppDbContext context, IConfiguration configuration): IAuthService
{
    public async Task<UserResponseDto?> RegisterUserAsync(UserRequestDto userRequestDto)
    {
        if (await context.Users.AnyAsync(u => u.Username == userRequestDto.Username))
        {
            return null;
        }

        var user = new User();
        
        var hashedPassword = new PasswordHasher<User>().HashPassword(user, userRequestDto.Password);
            
        user.Username = userRequestDto.Username;
        user.PasswordHash = hashedPassword;
        
        context.Users.Add(user);
        await context.SaveChangesAsync();
            
        return user.ToUserResponseDto();
    }

    public async Task<string?> LoginUserAsync(UserRequestDto userRequestDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == userRequestDto.Username);

        if (user == null)
        {
            return null;
        }
            
        var passwordVerification = new PasswordHasher<User>()
            .VerifyHashedPassword(user, user.PasswordHash, userRequestDto.Password);

        if (passwordVerification == PasswordVerificationResult.Failed)
        {
            return null;
        }

        string token = CreateToken(user);
            
        return token;
    }
    
    private string CreateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
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