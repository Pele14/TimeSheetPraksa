using TimeSheet.Application.Requests.AuthRequests;
using TimeSheet.Application.Responses.AuthResponses;

namespace TimeSheet.Application.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest dto);

    Task<string> ChangePasswordAsync(string username, string oldPassword, string newPassword);

    Task<string> GetUserAsync(string username);


}