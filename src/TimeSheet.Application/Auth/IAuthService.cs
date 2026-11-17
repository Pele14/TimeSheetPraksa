using TimeSheet.Application.Auth.DTOs;

namespace TimeSheet.Application.Auth;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDTO dto);

    Task<string> ChangePasswordAsync(string username, string oldPassword, string newPassword);

    Task<string> GetUserAsync(string username);


}