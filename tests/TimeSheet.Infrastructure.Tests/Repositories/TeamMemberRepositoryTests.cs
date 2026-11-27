

using System.Transactions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal;
using SQLitePCL;
using Timesheet.Domain.Entities;
using TimeSheet.Infrastructure.Repositories;
using TimeSheet.Infrastructure.Tests.Common;

namespace TimeSheet.Infrastructure.Tests.Repositories;





[TestFixture]
public class TeamMemberRepositoryTests
{
    private TestDatabase database=null!;

    [SetUp]
    public void SetUp()
    {
        database=new TestDatabase();

    }

    [TearDown]
    public void TearDown()
    {
        database.Dispose();
    }

    [Test]
    public async Task AddAsync_()
    {
        var member=TestData.CreateMember();

        using(var context = database.CreateContext())
        {
            var repository=new TeamMemberRepository(context);
            await repository.AddAsync(member);
        }
        
        using(var context= database.CreateContext())
        {
            var repository=new TeamMemberRepository(context);
            var result=await repository.GetByIdAsync(member.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(member.Id));
            Assert.That(result!.Name, Is.EqualTo(member.Name));
            Assert.That(result!.Email, Is.EqualTo(member.Email));
            Assert.That(result!.PasswordHash, Is.EqualTo(member.PasswordHash));
            Assert.That(result!.Role, Is.EqualTo(member.Role));
        }

    }
    [Test]
    public async Task UpdateAsync_KeepsPasswordHash()
    {
        
        var member=await AddMemberAsync();

        using(var context = database.CreateContext())
        {
            var repository=new TeamMemberRepository(context);
            var loaded=await repository.GetByIdAsync(member.Id);

            loaded!.Update("Marko", "marko123", "marko@gmail.com", 35, Role.User, MemberStatus.Inactive);
            await repository.UpdateAsync(loaded);
        }
        using(var context= database.CreateContext())
        {
            var result=await new TeamMemberRepository(context).GetByIdAsync(member.Id);

            Assert.That(result!.Name, Is.EqualTo("Marko"));
            Assert.That(result.Email, Is.EqualTo("marko@gmail.com"));
            Assert.That(result.HoursPerWeek, Is.EqualTo(35));
            Assert.That(result.Role, Is.EqualTo(Role.User));
            Assert.That(result.Status, Is.EqualTo(MemberStatus.Inactive));
            Assert.That(result.PasswordHash, Is.EqualTo(member.PasswordHash));
        }

    }

    [Test]
    public async Task DeleteAsync_()
    {
        var member=await AddMemberAsync();

        using(var context = database.CreateContext())
        {
            await new TeamMemberRepository(context).DeleteAsync(member.Id);

        }
        using(var context = database.CreateContext())
        {
            var row=await context.TeamMembers
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .SingleAsync(t=> t.Id==member.Id);
            Assert.That(row.IsDeleted, Is.True);
            Assert.That(row.DeletedAt, Is.Not.Null);

            var result=await new TeamMemberRepository(context).GetByIdAsync(member.Id);
            Assert.That(result, Is.Null);
        }
    }
    
    [Test]
    public async Task DeleteAsync_DeletesOwnEntries()
    {
        var lead=await AddMemberAsync("aleksa.pele03@gmail.com", "aleksa");
        var other=await AddMemberAsync("nikola@gmail.com", "nikola");
        var client=TestData.CreateClient();
        var project=TestData.CreateProject(client.Id, lead.Id);
        var category=TestData.CreateCategory();
        var leadEntry=TestData.CreateTimeEntry(project.Id, category.Id, lead.Id);
        var otherEntry=TestData.CreateTimeEntry(project.Id, category.Id, other.Id);


        using(var context= database.CreateContext())
        {
            await new ClientRepository(context).AddAsync(client);
            await new ProjectRepository(context).AddAsync(project);
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var entries=new TimeEntryRepository(context);
            await entries.AddAsync(leadEntry);
            await entries.AddAsync(otherEntry);
        }
        using(var context = database.CreateContext())
        {
            await new TeamMemberRepository(context).DeleteAsync(lead.Id);
        }
        using(var context = database.CreateContext())
        {
            var leadEntryRow=await context.TimeEntries.IgnoreQueryFilters().AsNoTracking()
                .SingleAsync(te=> te.Id==leadEntry.Id);
            var otherEntryRow=await context.TimeEntries.IgnoreQueryFilters().AsNoTracking()
                .SingleAsync(te=> te.Id==otherEntry.Id);
            var projectRow=await context.Projects.IgnoreQueryFilters().AsNoTracking()
                .SingleAsync(p=> p.Id==project.Id);

        Assert.That(leadEntryRow.IsDeleted, Is.True);
        Assert.That(otherEntryRow.IsDeleted, Is.False);
        Assert.That(projectRow.IsDeleted, Is.False);

        }


    }

    [Test]
    public async Task ExistsByEmailAsync()
    {
        await AddMemberAsync(email: "aleksa@gmail.com");

        using var context=database.CreateContext();
        var exists=await new TeamMemberRepository(context).ExistsByEmailAsync(" ALEKSA@gmail.com ");

        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task ExistByEmailAsync_DeletedMember()
    {
        var member=await AddMemberAsync(email: "aleksa@gmail.com");

        using(var context = database.CreateContext())
        {
            await new TeamMemberRepository(context).DeleteAsync(member.Id);
        }
        using (var context= database.CreateContext())
        {
            var exists= await new TeamMemberRepository(context).ExistsByEmailAsync("aleksa@gmail.com");

            Assert.That(exists, Is.False);
        }

    }
    [Test]
    public async Task AddAsync_SetsCreatedAt()
    {
        var member=await AddMemberAsync();

        DateTime createdAt;
        using(var context= database.CreateContext())
        {
            var row=await context.TeamMembers.SingleAsync(t => t.Id==member.Id);

            Assert.That(row.CreatedAt, Is.Not.EqualTo(default(DateTime)));
            Assert.That(row.UpdatedAt, Is.Null);

            createdAt=row.CreatedAt;

        }
        using(var context = database.CreateContext())
        {
            var repository= new TeamMemberRepository(context);
            var loaded=await repository.GetByIdAsync(member.Id);
            loaded!.Update("Marko", "marko123", "marko@gmail.com", 35, Role.User, MemberStatus.Active);
            await repository.UpdateAsync(loaded);
        }

        using(var context = database.CreateContext())
        {
            var row=await context.TeamMembers.SingleAsync(t=> t.Id==member.Id);

            Assert.That(row.UpdatedAt, Is.Not.Null);
            Assert.That(row.CreatedAt, Is.EqualTo(createdAt));

        }
    }
    [Test]
    public async Task GetByEmailAsync_Uppdercase()
    {
        var member=await AddMemberAsync(email: "aleksa@gmail.com");

        using var context=database.CreateContext();
        var result=await new TeamMemberRepository(context).GetByEmailAsync("ALEKSA@GMAIL.Com");
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(member.Id));
    }
    [Test]
    public async Task GetAllAsync_ExcludeDeleted()
    {
        var member=await AddMemberAsync(email: "aleksa@gmail.com", "aleksa");
        var deleted=await AddMemberAsync(email: "marko@gmail.com", "marko");

        using (var context = database.CreateContext())
        {
            await new TeamMemberRepository(context).DeleteAsync(deleted.Id);

        }
        using (var context = database.CreateContext())
        {
            var result=await new TeamMemberRepository(context).GetAllAsync();

            Assert.That(result.Select(m=> m.Id), Is.EquivalentTo(new[]{ member.Id}));
        }
    }

    [Test]
    public async Task GetById_NonExistingId()
    {
        using var context=database.CreateContext();
        var result=await new TeamMemberRepository(context).GetByIdAsync(Guid.NewGuid());
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddAsync_DuplicateEmail()
    {
        await AddMemberAsync(email: "aleksa@gmail.com");

        using var context= database.CreateContext();
        var repository=new TeamMemberRepository(context);

        Assert.ThrowsAsync<DbUpdateException>(()=>
            repository.AddAsync(TestData.CreateMember(email: "aleksa@gmail.com")));

    }




private async Task<TeamMemberModel> AddMemberAsync(string email = "aleksa@gmail.com", string username="aleksa123")
{
    var member = TestData.CreateMember(email, username);

    using var context = database.CreateContext();
    await new TeamMemberRepository(context).AddAsync(member);

    return member;
}

}