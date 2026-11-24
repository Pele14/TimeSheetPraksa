using MediatR;
using TimeSheet.Application.Requests.TimeEntryRequests;

namespace TimeSheet.Application.TimeEntry.Commands.UpdateTimeEntry;


public record UpdateTimeEntryCommand(Guid Id, UpdateTimeEntryRequest Request) : IRequest;