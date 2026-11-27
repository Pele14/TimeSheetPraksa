using System.Net.NetworkInformation;
using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.TimeEntry.Queries.GetDayTimeEntries;
using TimeSheet.Application.TimeEntry.Queries.GetTimeEntryById;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.TimeEntries.Queries;

[TestFixture]
public class GetDayTimeEntriesHandlerTests
{
    private GetDayTimeEntriesTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new GetDayTimeEntriesTestContext();
    }

    [Test]
    public async Task Handle_DayWithEntries()
    {
        context.WithEntries(
                    context.CreateEntry(hours: 4, overtime: null),
                    context.CreateEntry(hours: 3, overtime: 0.5))
               .WithLookups();
        
        var result=await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        Assert.That(result.Date, Is.EqualTo(GetDayTimeEntriesTestContext.Date));
        Assert.That(result.TotalHours, Is.EqualTo(7.5));
        Assert.That(result.TimeEntries, Has.Count.EqualTo(2));
        Assert.That(result.TimeEntries[0].ProjectName, Is.EqualTo(GetDayTimeEntriesTestContext.ProjectName));
        Assert.That(result.TimeEntries[0].ClientName, Is.EqualTo(GetDayTimeEntriesTestContext.ClientName));
        Assert.That(result.TimeEntries[0].CategoryName, Is.EqualTo(GetDayTimeEntriesTestContext.CategoryName));
    }
    [Test]
    public async Task Handle_DayWithoutEntries()
    {
        context.WithEntries()
                .WithLookups();
        
        var result=await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);
        
        Assert.That(result.TotalHours, Is.EqualTo(0));
        Assert.That(result.TimeEntries, Is.Empty);
    }

    [Test]
    public async Task Handle_UsesCurrentUserAndSamDay()
    {
        context.WithEntries()
               .WithLookups();

        var result=await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        context.TimeEntryRepositoryMock.Verify(r=> r.GetByMemberAndDateRangeAsync(
            context.memberId,
            GetDayTimeEntriesTestContext.Date,
            GetDayTimeEntriesTestContext.Date), Times.Once);


    }
}

public class GetDayTimeEntriesTestContext
{  
    public const string ProjectName="Project";
    public const string ClientName="Aleksa";
    public const string CategoryName="Backend Dev";
    public static readonly DateOnly Date=new(2026, 11, 25);
    public Guid categoryId{get; }=Guid.NewGuid();
    public Guid projectId{get; }=Guid.NewGuid();
    public Guid clientId{get; }=Guid.NewGuid();
    public Guid memberId{get; }=Guid.NewGuid();

    public Mock<ITimeEntryRepository> TimeEntryRepositoryMock { get; } = new();
    public Mock<IClientRepository> ClientRepositoryMock { get; } = new();
    public Mock<ICategoryRepository> CategoryRepositoryMock { get; } = new();
    public Mock<IProjectRepository> ProjectRepositoryMock { get; } = new();
    public Mock<ICurrentUserService> CurrentUserServiceMock{get; }= new();
    public GetDayTimeEntriesHandler Handler { get; }

    public GetDayTimeEntriesTestContext()
    {   
        CurrentUserServiceMock.Setup(s=> s.UserId).Returns(memberId);
        Handler = new GetDayTimeEntriesHandler(TimeEntryRepositoryMock.Object, ProjectRepositoryMock.Object, ClientRepositoryMock.Object, CategoryRepositoryMock.Object, CurrentUserServiceMock.Object);

    }

    public GetDayTimeEntriesQuery CreateQuery()
    {
        return new GetDayTimeEntriesQuery(Date);
    }

    public TimeEntryModel CreateEntry(double hours, double? overtime)
    {
        return new TimeEntryModel(Guid.NewGuid(), Date, hours, overtime, "Description",
            project: projectId,
            category: categoryId,
            teammember: memberId);
    }
    public GetDayTimeEntriesTestContext WithEntries(params TimeEntryModel[] entries)
    {
        TimeEntryRepositoryMock
            .Setup(r=>r.GetByMemberAndDateRangeAsync(memberId, Date, Date))
            .ReturnsAsync(entries.ToList());
        return this;
    }
    public GetDayTimeEntriesTestContext WithLookups()
    {
        ProjectRepositoryMock
            .Setup(r=> r.GetAllAsync(true))
            .ReturnsAsync(new List<ProjectModel>
            {
                new(projectId, ProjectName, "Description", ProjectStatus.Active, clientId, memberId)
            });
        ClientRepositoryMock
            .Setup(r=> r.GetAllAsync())
            .ReturnsAsync(new List<ClientModel>
            {
                new(clientId, ClientName, "Bulevar", "Novi Sad", "21000", Country.Serbia)
            });
        CategoryRepositoryMock
            .Setup(r=> r.GetAllAsync())
            .ReturnsAsync(new List<CategoryModel>
            {
                new(categoryId, CategoryName)
            });
        return this;    
    }

}