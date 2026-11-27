using MediatR;
using TimeSheet.Application.Responses.TimeEntryResponse;
using TimeSheet.Application.TimeEntry.Queries.GetMonthTimeEntries;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.TimeEntry.Queries.GetMonthTimeEntries;

public class GetTimeEntriesSummaryHandler : IRequestHandler<GetTimeEntriesSummaryQuery, MonthTimeEntriesResponse>
{
    private readonly ITimeEntryRepository TimeEntryRepository;
    private readonly ICurrentUserService CurrentUserService;

    public GetTimeEntriesSummaryHandler(ITimeEntryRepository timeEntryRepository, ICurrentUserService currentUserService)
    {
        TimeEntryRepository=timeEntryRepository;
        CurrentUserService=currentUserService;

    }
    public async Task<MonthTimeEntriesResponse> Handle(GetTimeEntriesSummaryQuery query, CancellationToken cancellationToken)
    {
        var request=query.Request;

        var entries=await TimeEntryRepository.GetByMemberAndDateRangeAsync(CurrentUserService.UserId, request.From, request.To);

        var hoursByDay=entries
                .GroupBy(e=> e.Date)
                .ToDictionary(g=> g.Key, g=>g.Sum(e=>e.Hours +(e.Overtime?? 0)));
        var days=new List<DaySummaryResponse>();
        for(var day=request.From; day<=request.To; day=day.AddDays(1))
        {
            days.Add(new DaySummaryResponse
            {
                Date=day,
                TotalHours=hoursByDay.GetValueOrDefault(day, 0)   

            });
        }
        return new MonthTimeEntriesResponse
        {
            TotalHours=days.Sum(d=>d.TotalHours),
            Days=days

        };
    }
}