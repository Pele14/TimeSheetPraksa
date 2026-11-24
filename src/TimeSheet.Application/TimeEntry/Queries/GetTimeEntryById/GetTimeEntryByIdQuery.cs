
using MediatR;
using TimeSheet.Application.Responses.TimeEntryResponse;

namespace TimeSheet.Application.TimeEntry.Queries.GetTimeEntryById;


public record GetTimeEntryByIdQuery(Guid id) : IRequest<TimeEntryResponse>;