namespace TimeSheet.Application.Common;

    public interface ITokenService
    {
        string GenerateToken(Guid userId, string role);
    }