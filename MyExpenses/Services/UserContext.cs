using System.Security.Claims;

namespace MyExpenses.Services;

public class UserContext(IHttpContextAccessor accessor): IUserContext
{
    public Guid UserId
    {
        get
        {
            var id = accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var guid) ? guid : Guid.Empty;
        }
    }
}