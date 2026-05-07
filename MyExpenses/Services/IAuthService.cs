using MyExpenses.Dtos;
using MyExpenses.Entities;

namespace MyExpenses.Services;

public interface IAuthService
{
    public Task<User?> RegisterUserAsync(UserDto userDto);
    public Task<string?> LoginUserAsync(UserDto userDto);
}