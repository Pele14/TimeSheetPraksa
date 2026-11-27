
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using Timesheet.Domain.Entities;
using TimeSheet.Core.Reports;
using TimeSheet.Infrastructure.Repositories;
using TimeSheet.Infrastructure.Tests.Common;

namespace TimeSheet.Infrastructure.Tests.Repositories;

[TestFixture]
public class ReportRepositoryTests
{
    private TestDatabase database=null!;
    private TeamMemberModel aleksa=null!;
    private TeamMemberModel nikola=null!;
    private ClientModel vegaIT=null!;
    private ClientModel continental=null!;
    private ProjectModel pr1=null!;
    private ProjectModel pr2=null!;

    [SetUp]
    public async Task SetUp()
    {
        database=new TestDatabase();

        aleksa=TestData.CreateMember("aleksa.pele03@gmail.com", "aleksa", name: "Aleksa");
        nikola=TestData.CreateMember("nikola@gmail.com", "nikola", name: "Nikola");
        vegaIT=TestData.CreateClient("VegaIT");
        continental=TestData.CreateClient("Continental");
        pr1=TestData.CreateProject(vegaIT.Id, aleksa.Id, "Projekat1");
        pr2=TestData.CreateProject(continental.Id, nikola.Id, "Projekat2");
        var category=TestData.CreateCategory();

        using var context=database.CreateContext();
        await new TeamMemberRepository(context).AddAsync(aleksa);
        await new TeamMemberRepository(context).AddAsync(nikola);
        await new ClientRepository(context).AddAsync(vegaIT);
        await new ClientRepository(context).AddAsync(continental);
        await new ProjectRepository(context).AddAsync(pr1);
        await new ProjectRepository(context).AddAsync(pr2);

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var entries=new TimeEntryRepository(context);
        await entries.AddAsync(TestData.CreateTimeEntry(pr1.Id, category.Id, aleksa.Id, new DateOnly(2026, 11, 25)));
        await entries.AddAsync(TestData.CreateTimeEntry(pr2.Id, category.Id, nikola.Id, new DateOnly(2026, 11, 25)));
        await entries.AddAsync(TestData.CreateTimeEntry(pr1.Id, category.Id, nikola.Id, new DateOnly(2026, 12, 1)));
    }
    
    [TearDown]
    public void TearDown()=> database.Dispose();

    private async Task<List<ReportItem>> GetReportAsync(ReportFilter filter)
    {
        using var context=database.CreateContext();
        return await new ReportRepository(context).GetReportAsync(filter);

    }

    [Test]
    public async Task GetReportAsync_NoFilters()
    {
        var result=await GetReportAsync(new ReportFilter());

        Assert.That(result, Has.Count.EqualTo(3));

        var first=result[0];
        Assert.That(first.Date, Is.EqualTo(new DateOnly(2026, 11, 25)));
        Assert.That(first.TeamMemberName, Is.EqualTo(aleksa.Name));
        Assert.That(first.ClientName, Is.EqualTo("VegaIT"));
        Assert.That(first.ProjectName, Is.EqualTo("Projekat1"));
        Assert.That(first.Time, Is.EqualTo(8));

    }
    [Test]
    public async Task GetReportAsync_ByTeamMember_ReturnsOnlyMemberEntries()
    {
        var result = await GetReportAsync(new ReportFilter { TeamMemberId = nikola.Id });

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.All(i => i.TeamMemberName == nikola.Name), Is.True);
    }

    [Test]
    public async Task GetReportAsync_ByClient_ReturnsOnlyClientEntries()
    {
        var result = await GetReportAsync(new ReportFilter { ClientId = continental.Id });

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].ClientName, Is.EqualTo("Continental"));
    }

    [Test]
    public async Task GetReportAsync_ByDateRange_ReturnsOnlyEntriesInRange()
    {
        var result = await GetReportAsync(new ReportFilter
        {
            StartDate = new DateOnly(2026, 11, 1),
            EndDate = new DateOnly(2026, 11, 30)
        });

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.All(i => i.Date.Month == 11), Is.True);
    }

    [Test]
    public async Task GetReportAsync_CombinedFilters_ReturnsIntersection()
    {
        var result = await GetReportAsync(new ReportFilter
        {
            ProjectId = pr1.Id,
            TeamMemberId = nikola.Id
        });

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Date, Is.EqualTo(new DateOnly(2026, 12, 1)));
    }
}

