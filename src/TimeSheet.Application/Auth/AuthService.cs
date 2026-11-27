using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.AuthRequests;
using TimeSheet.Application.Responses.AuthResponses;
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


    public async Task<LoginResponse> LoginAsync(LoginRequest request)
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
        else if(entity.Status!=MemberStatus.Active)
             throw new ForbiddenException("Your account is inactive");
        
        var token = TokenService.GenerateToken(entity.Id, entity.Role.ToString());
        return new LoginResponse
        {
            Token=token,
            Id=entity.Id,
            Name=entity.Name,
            Email=entity.Email,
            Role=entity.Role
        };

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