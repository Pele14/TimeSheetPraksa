

using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Timesheet.Domain.Entities;
using TimeSheet.Domain.Enums;
using TimeSheet.Infrastructure.Repositories;
using TimeSheet.Infrastructure.Tests.Common;

namespace TimeSheet.Infrastructure.Tests.Repositories;





[TestFixture]
public class TimeEntryRepositoryTests
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
        var (projectId, categoryId, memberId)=await AddParentsAsync();
        var TimeEntry=await AddTimeEntryAsync(projectId, categoryId, memberId);

        using(var context= database.CreateContext())
        {
            var repository=new TimeEntryRepository(context);
            var result=await repository.GetByIdAsync(TimeEntry.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Date, Is.EqualTo(TimeEntry.Date));
            Assert.That(result.Hours, Is.EqualTo(7.5));
            Assert.That(result.Overtime, Is.EqualTo(0.5));
            Assert.That(result.ProjectId, Is.EqualTo(projectId));
            Assert.That(result.CategoryId, Is.EqualTo(categoryId));
            Assert.That(result.TeamMemberId, Is.EqualTo(memberId));
        }

    }
    [Test]
    public async Task UpdateAsync_()
    {
        var (projectId, categoryId, memberId)=await AddParentsAsync();
        var TimeEntry=await AddTimeEntryAsync(projectId, categoryId, memberId);

        using(var context= database.CreateContext())
        {
            var repository=new TimeEntryRepository(context);
            var loaded=await repository.GetByIdAsync(TimeEntry.Id);

            loaded!.Update(new DateOnly(2026, 11, 26), 4, null, "Changed", projectId: projectId, categoryId: categoryId);
            await repository.UpdateAsync(loaded);
        }
        using(var context= database.CreateContext())
        {
            var result=await new TimeEntryRepository(context).GetByIdAsync(TimeEntry.Id);

            Assert.That(result!.Date, Is.EqualTo(new DateOnly(2026, 11, 26)));
            Assert.That(result.Hours, Is.EqualTo(4));
            Assert.That(result.Overtime, Is.Null);
            Assert.That(result.Description, Is.EqualTo("Changed"));
            Assert.That(result.TeamMemberId, Is.EqualTo(memberId));
        }

    }

    [Test]
    public async Task DeleteAsync_()
    {
        var(projectId, categoryId, memberId)=await AddParentsAsync();
        var TimeEntry=await AddTimeEntryAsync(projectId, categoryId, memberId);

        using(var context = database.CreateContext())
        {
            await new TimeEntryRepository(context).DeleteAsync(TimeEntry.Id);

        }
        using(var context = database.CreateContext())
        {
            var row=await context.TimeEntries
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .SingleAsync(t=> t.Id==TimeEntry.Id);
            Assert.That(row.IsDeleted, Is.True);
        }
        using(var context=database.CreateContext()){

            var result=await new TimeEntryRepository(context).GetByIdAsync(TimeEntry.Id);
            Assert.That(result, Is.Null);
        }
    }



    private async Task<TimeEntryModel> AddTimeEntryAsync(Guid projectId, Guid categoryId, Guid memberId)
    {
        var TimeEntry = TestData.CreateTimeEntry(projectId, categoryId, memberId);

        using var context = database.CreateContext();
        await new TimeEntryRepository(context).AddAsync(TimeEntry);

        return TimeEntry;
    }
    private async Task<(Guid projectId, Guid categoryId, Guid memberId)>AddParentsAsync()
    {
        var client=TestData.CreateClient();
        var member=TestData.CreateMember();
        var project=TestData.CreateProject(client.Id, member.Id);
        var category=TestData.CreateCategory();

        using var context=database.CreateContext();
        await new ClientRepository(context).AddAsync(client);
        await new TeamMemberRepository(context).AddAsync(member);
        await new ProjectRepository(context).AddAsync(project);

        context.Categories.Add(category);
        await context.SaveChangesAsync();
            
        return (project.Id, category.Id, member.Id);
    }
}