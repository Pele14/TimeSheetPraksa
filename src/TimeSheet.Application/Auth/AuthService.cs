
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.Auth;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;

    public AuthService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<string> LoginAsync(string username, string password)
    {
      
        var userId = Guid.NewGuid().ToString(); 
        var role = "User"; 
        return _tokenService.GenerateToken(userId, role);
    }

    public async Task<string> ChangePasswordAsync(string username, string oldPassword, string newPassword)
    {
        // Implement your change password logic here
        // For demonstration purposes, let's assume the password change is successful and return a success message.
        return "Password changed successfully.";
    }

    public async Task<string> GetUserAsync(string username)
    {
        // Implement your get user logic here
        // For demonstration purposes, let's assume we retrieved the user information and return it as a string.
        return $"User information for {username}.";
    }
}