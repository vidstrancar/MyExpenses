using MyExpenses.Entities;

namespace MyExpenses.Dtos;

public class UserResponseDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
}

public static class UserExtensions
{
    extension(User user)
    {
        public UserResponseDto ToUserResponseDto()
        {
            return new UserResponseDto
            {
                UserId = user.UserId,
                Username = user.Username
            };
        }
    }
}