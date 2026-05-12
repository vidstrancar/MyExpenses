using System.Security.Claims;

namespace MyExpenses.Services;

public class UserContext(IHttpContextAccessor accessor): IUserContext
{
    public Guid UserId => 
        Guid.TryParse(
            accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), 
            out var guid) ? 
        guid : 
        throw new UnauthorizedAccessException("User is not authenticated");
}