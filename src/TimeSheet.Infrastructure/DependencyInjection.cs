using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeSheet.Domain.Interfaces;
using TimeSheet.Infrastructure.Persistence;
using TimeSheet.Infrastructure.Repositories;

namespace TimeSheet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
        return services;
    }
}