namespace TimeSheet.Domain.Interfaces;

    public interface ITokenService
    {
        string GenerateToken(string userId, string role);
    }