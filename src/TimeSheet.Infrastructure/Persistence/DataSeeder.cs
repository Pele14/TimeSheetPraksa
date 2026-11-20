
using System.Net;
using Microsoft.EntityFrameworkCore;
using Timesheet.Infrastructure.DBEntities;
using TimeSheet.Core.Security;

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

    public async Task SeedAdminAsync()
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


    public async Task SeedCategoriesAsync()
    {
        var exists=await Db.Categories
                .IgnoreQueryFilters()
                .AnyAsync();
        if(exists)
            return;

        var names = new[]
        {
            "Frontend Development",
            "Backend Development",
            "Desing",
            "Testing",
            "Meeting",
            "Project Management"
        };
        foreach(var name in names)
        {
            Db.Categories.Add(new Category
            {
                Id=Guid.NewGuid(),
                Name=name


            });


        }

    }
}