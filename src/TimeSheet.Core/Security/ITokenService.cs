namespace TimeSheet.Core.Security;

public interface ITokenService
{
    string GenerateToken(Guid userId, string role);
}