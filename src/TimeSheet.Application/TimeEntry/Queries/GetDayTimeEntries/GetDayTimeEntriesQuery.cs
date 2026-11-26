using MediatR;
using TimeSheet.Application.Responses.TimeEntryResponse;

namespace TimeSheet.Application.TimeEntry.Queries.GetDayTimeEntries;


public record GetDayTimeEntriesQuery(DateOnly Date): IRequest<DayTimeEntriesResponse>;