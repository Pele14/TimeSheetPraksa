namespace TimeSheet.Application.Auth;

public interface IAuthService
{
    Task<string> LoginAsync(string username, string password);

    Task<string> ChangePasswordAsync(string username, string oldPassword, string newPassword);

    Task<string> GetUserAsync(string username);


}