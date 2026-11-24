
using MediatR;
using TimeSheet.Application.Requests.TimeEntryRequests;
using TimeSheet.Application.Responses.TimeEntryResponse;


namespace TimeSheet.Application.TimeEntry.Commands.CreateTimeEntry;


public record CreateTimeEntryCommand(CreateTimeEntryRequest Request) : IRequest<TimeEntryResponse>;

