using System.Net.NetworkInformation;
using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.TimeEntryRequests;
using TimeSheet.Application.TimeEntry.Queries.GetMonthTimeEntries;
using TimeSheet.Application.TimeEntry.Queries.GetTimeEntryById;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Tests.TimeEntries.Queries;

[TestFixture]
public class GetMonthTimeEntriesHandlerTests
{
    private GetMonthTimeEntriesTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new GetMonthTimeEntriesTestContext();
    }

    [Test]
    public async Task Handle_ReturnsAllDays()
    {
        context.WithEntries();
        
        var result=await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        Assert.That(result.Days, Has.Count.EqualTo(30));
        Assert.That(result.Days.First().Date, Is.EqualTo(GetMonthTimeEntriesTestContext.FirstDay));
        Assert.That(result.Days.Last().Date, Is.EqualTo(GetMonthTimeEntriesTestContext.LastDay));
        Assert.That(result.Days.All(d=>d.TotalHours==0), Is.True);
        Assert.That(result.TotalHours, Is.EqualTo(0));
    }
    [Test]
    public async Task Handle_SumsEntries()
    {
        var day1=new DateOnly(2026, 11, 25);
        var day2=new DateOnly(2026, 11, 26);

        context.WithEntries(
            context.CreateEntry(day1, hours: 4, overtime: null),
            context.CreateEntry(day1, hours: 3, overtime: 0.5),
            context.CreateEntry(day2, hours: 8, overtime: null));
        
        var result=await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);
        
        Assert.That(result.Days.Single(d=> d.Date==day1).TotalHours, Is.EqualTo(7.5));
        Assert.That(result.Days.Single(d=> d.Date==day2).TotalHours, Is.EqualTo(8));
        Assert.That(result.Days.Single(d=> d.Date==new DateOnly(2026, 11, 24)).TotalHours, Is.EqualTo(0));
        Assert.That(result.TotalHours, Is.EqualTo(15.5));
    }

    [Test]
    public async Task Handle_UsesCurrentUser()
    {
        context.WithEntries();

        await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        context.TimeEntryRepositoryMock.Verify(r=> r.GetByMemberAndDateRangeAsync(
            context.memberId,
            GetMonthTimeEntriesTestContext.FirstDay,
            GetMonthTimeEntriesTestContext.LastDay), Times.Once);   

    }
}

public class GetMonthTimeEntriesTestContext
{  
    public const int Year=2026;
    public const int Month=11;
    public static readonly DateOnly FirstDay=new(2026, 11, 1); 
    public static readonly DateOnly LastDay=new(2026, 11, 30);
    public Guid memberId{get; }=Guid.NewGuid();

    public Mock<ITimeEntryRepository> TimeEntryRepositoryMock { get; } = new();
    public Mock<ICurrentUserService> CurrentUserServiceMock{get; }= new();
    public GetMonthTimeEntriesHandler Handler { get; }

    public GetMonthTimeEntriesTestContext()
    {   
        CurrentUserServiceMock.Setup(s=> s.UserId).Returns(memberId);
        Handler = new GetMonthTimeEntriesHandler(TimeEntryRepositoryMock.Object, CurrentUserServiceMock.Object);

    }

    public GetMonthTimeEntriesQuery CreateQuery()
    {
        return new GetMonthTimeEntriesQuery(new GetMonthTimeEntriesRequest
        {
            Year=Year,
            Month=Month
        });
    }

    public TimeEntryModel CreateEntry(DateOnly date, double hours, double? overtime)
    {
        return new TimeEntryModel(Guid.NewGuid(), date, hours, overtime, "Description",
            project: Guid.NewGuid(),
            category: Guid.NewGuid(),
            teammember: memberId);
    }
    public GetMonthTimeEntriesTestContext WithEntries(params TimeEntryModel[] entries)
    {
        TimeEntryRepositoryMock
            .Setup(r=>r.GetByMemberAndDateRangeAsync(memberId, FirstDay, LastDay))
            .ReturnsAsync(entries.ToList());
        return this;
    }


}