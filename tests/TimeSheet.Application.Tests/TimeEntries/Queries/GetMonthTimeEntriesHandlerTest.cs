using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Requests.TimeEntryRequests;
using TimeSheet.Application.TimeEntry.Queries.GetMonthTimeEntries;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.Tests.TimeEntries.Queries;

[TestFixture]
public class GetTimeEntriesSummaryHandlerTests
{
    private GetTimeEntriesSummaryTestContext context = null!;

    [SetUp]
    public void SetUp()
    {
        context = new GetTimeEntriesSummaryTestContext();
    }

    [Test]
    public async Task Handle_ReturnsAllDaysInPeriod()
    {
        context.WithEntries();

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        Assert.That(result.Days, Has.Count.EqualTo(30));
        Assert.That(result.Days.First().Date, Is.EqualTo(GetTimeEntriesSummaryTestContext.From));
        Assert.That(result.Days.Last().Date, Is.EqualTo(GetTimeEntriesSummaryTestContext.To));
        Assert.That(result.Days.All(d => d.TotalHours == 0), Is.True);
        Assert.That(result.TotalHours, Is.EqualTo(0));
    }

    [Test]
    public async Task Handle_SumsEntries()
    {
        var day1 = new DateOnly(2026, 11, 25);
        var day2 = new DateOnly(2026, 11, 26);

        context.WithEntries(
            context.CreateEntry(day1, hours: 4, overtime: null),
            context.CreateEntry(day1, hours: 3, overtime: 0.5),
            context.CreateEntry(day2, hours: 8, overtime: null));

        var result = await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        Assert.That(result.Days.Single(d => d.Date == day1).TotalHours, Is.EqualTo(7.5));
        Assert.That(result.Days.Single(d => d.Date == day2).TotalHours, Is.EqualTo(8));
        Assert.That(result.Days.Single(d => d.Date == new DateOnly(2026, 11, 24)).TotalHours, Is.EqualTo(0));
        Assert.That(result.TotalHours, Is.EqualTo(15.5));
    }

    [Test]
    public async Task Handle_PeriodAcrossMonths()
    {
        var from = new DateOnly(2026, 10, 26);
        var to = new DateOnly(2026, 12, 6);

        context.WithEntries(from, to,
            context.CreateEntry(new DateOnly(2026, 10, 30), hours: 5, overtime: null),
            context.CreateEntry(new DateOnly(2026, 12, 2), hours: 6, overtime: null));

        var result = await context.Handler.Handle(context.CreateQuery(from, to), CancellationToken.None);

        Assert.That(result.Days, Has.Count.EqualTo(42));
        Assert.That(result.Days.First().Date, Is.EqualTo(from));
        Assert.That(result.Days.Last().Date, Is.EqualTo(to));
        Assert.That(result.TotalHours, Is.EqualTo(11));
    }

    [Test]
    public async Task Handle_UsesCurrentUserAndPeriod()
    {
        context.WithEntries();

        await context.Handler.Handle(context.CreateQuery(), CancellationToken.None);

        context.TimeEntryRepositoryMock.Verify(r => r.GetByMemberAndDateRangeAsync(
            context.memberId,
            GetTimeEntriesSummaryTestContext.From,
            GetTimeEntriesSummaryTestContext.To), Times.Once);
    }
}

public class GetTimeEntriesSummaryTestContext
{
    public static readonly DateOnly From = new(2026, 11, 1);
    public static readonly DateOnly To = new(2026, 11, 30);

    public Guid memberId { get; } = Guid.NewGuid();

    public Mock<ITimeEntryRepository> TimeEntryRepositoryMock { get; } = new();
    public Mock<ICurrentUserService> CurrentUserServiceMock { get; } = new();
    public GetTimeEntriesSummaryHandler Handler { get; }

    public GetTimeEntriesSummaryTestContext()
    {
        CurrentUserServiceMock.Setup(s => s.UserId).Returns(memberId);
        Handler = new GetTimeEntriesSummaryHandler(TimeEntryRepositoryMock.Object, CurrentUserServiceMock.Object);
    }

    public GetTimeEntriesSummaryQuery CreateQuery()
    {
        return CreateQuery(From, To);
    }

    public GetTimeEntriesSummaryQuery CreateQuery(DateOnly from, DateOnly to)
    {
        return new GetTimeEntriesSummaryQuery(new GetTimeEntriesSummaryRequest
        {
            From = from,
            To = to
        });
    }

    public TimeEntryModel CreateEntry(DateOnly date, double hours, double? overtime)
    {
        return new TimeEntryModel(Guid.NewGuid(), date, hours, overtime, "Description",
            project: Guid.NewGuid(),
            category: Guid.NewGuid(),
            teammember: memberId);
    }

    public GetTimeEntriesSummaryTestContext WithEntries(params TimeEntryModel[] entries)
    {
        return WithEntries(From, To, entries);
    }

    public GetTimeEntriesSummaryTestContext WithEntries(DateOnly from, DateOnly to, params TimeEntryModel[] entries)
    {
        TimeEntryRepositoryMock
            .Setup(r => r.GetByMemberAndDateRangeAsync(memberId, from, to))
            .ReturnsAsync(entries.ToList());
        return this;
    }
}