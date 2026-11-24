using MediatR;


namespace TimeSheet.Application.TimeEntry.Commands.DeleteTimeEntry;

public record DeleteTimeEntryCommand(Guid id) : IRequest;