using MyExpenses.Dtos;
using MyExpenses.Entities;

namespace MyExpenses.Services;

public interface IAuthService
{
    public Task<UserResponseDto?> RegisterUserAsync(UserRequestDto userRequestDto);
    public Task<string?> LoginUserAsync(UserRequestDto userRequestDto);
}