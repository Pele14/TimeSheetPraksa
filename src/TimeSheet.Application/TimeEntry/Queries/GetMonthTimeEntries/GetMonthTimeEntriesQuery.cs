using MediatR;
using TimeSheet.Application.Requests.TimeEntryRequests;
using TimeSheet.Application.Responses.TimeEntryResponse;

namespace TimeSheet.Application.TimeEntry.Queries.GetMonthTimeEntries;



public record GetMonthTimeEntriesQuery(GetMonthTimeEntriesRequest Request): IRequest<MonthTimeEntriesResponse>;