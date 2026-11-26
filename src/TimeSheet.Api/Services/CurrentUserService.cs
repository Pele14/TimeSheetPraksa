
using System.Security.Claims;
using TimeSheet.Core.Security;

namespace TimeSheet.Api.Services;
public class CurrentUserService: ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor=httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var value=_httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(value, out var id)
                    ? id
                    : throw new UnauthorizedAccessException("User is not authenticated");
        }
    }
}