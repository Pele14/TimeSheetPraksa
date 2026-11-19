
using TimeSheet.Domain.Interfaces;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TimeSheet.Application.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;




namespace TimeSheet.Infrastructure.Security;

public class TokenService : ITokenService
{

    private readonly JwtSettings jwtSettings;
    private readonly SigningCredentials signingCredentials;
    public TokenService(IOptions<JwtSettings> jwtSettingsOptions)
    {
        jwtSettings = jwtSettingsOptions.Value;
        var keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }

    public string GenerateToken(Guid userId, string role)
    {

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Role, role)

                }),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes),
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            SigningCredentials = signingCredentials
        };
        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }
}

