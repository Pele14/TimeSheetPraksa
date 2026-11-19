
using TimeSheet.Application.Auth.DTOs;

using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.Auth;

public class AuthService : IAuthService
{

    private readonly ITokenService TokenService;
    private readonly IPasswordHasher PasswordHasher;

    private readonly ITeamMemberRepository TeamMemberRepository;
    public AuthService(ITokenService tokenService, IPasswordHasher passwordHasher, ITeamMemberRepository tm)
    {
        TokenService = tokenService;
        PasswordHasher = passwordHasher;
        TeamMemberRepository = tm;

    }


    public async Task<string> LoginAsync(LoginDTO request)
    {   
        var email=request.Email.Trim().ToLowerInvariant();
        var entity = await TeamMemberRepository.GetByEmailAsync(email);
        if (entity is null )
        {
            throw new UnauthorizedException("Wrong email");

        }
        else if (!PasswordHasher.Verify(request.Password, entity.PasswordHash))
        {
            throw new UnauthorizedException("Wrong password");

        }
        var token = TokenService.GenerateToken(entity.Id, entity.Role.ToString());
        return token;

    }

    public async Task<string> ChangePasswordAsync(string username, string oldPassword, string newPassword)
    {

        return "";
    }

    public async Task<string> GetUserAsync(string username)
    {

        return "";
    }


}