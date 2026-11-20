using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;
using TimeSheet.Infrastructure.Persistence;
using TimeSheet.Infrastructure.Repositories;
using TimeSheet.Infrastructure.Security;
namespace TimeSheet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<DataSeeder>();
        return services;

    }
}