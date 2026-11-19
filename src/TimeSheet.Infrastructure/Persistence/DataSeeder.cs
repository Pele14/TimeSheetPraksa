
using System.Net;
using Microsoft.EntityFrameworkCore;
using Timesheet.Infrastructure.DBEntities;
using TimeSheet.Application.Common;

namespace TimeSheet.Infrastructure.Persistence;


public class DataSeeder
{
    private readonly AppDbContext Db;
    private readonly IPasswordHasher PasswordHasher;


    public DataSeeder(AppDbContext db, IPasswordHasher passwordHasher)
    {
        Db = db;
        PasswordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        var email = "admin@gmail.com";
        var exists = await Db.TeamMembers.IgnoreQueryFilters().AnyAsync(t => t.Email == email);

        if (exists)
            return;

        Db.TeamMembers.Add(new TeamMember
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Username = "Admin123",
            Email = email,
            PasswordHash = PasswordHasher.Hash("123"),
            HoursPerWeek = 7,
            Role = Role.Admin,
            Status = MemberStatus.Active

        });
        await Db.SaveChangesAsync();
    }
}