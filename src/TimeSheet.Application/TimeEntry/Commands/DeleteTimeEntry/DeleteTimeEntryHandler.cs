
using MediatR;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.TimeEntry.Commands.DeleteTimeEntry;


public class DeleteTimeEntryHandler : IRequestHandler<DeleteTimeEntryCommand>
{
    private readonly ITimeEntryRepository TimeEntryRepository;

    public DeleteTimeEntryHandler(ITimeEntryRepository timeEntryRepository)
    {
        TimeEntryRepository = timeEntryRepository;
    }



    public async Task Handle(DeleteTimeEntryCommand request, CancellationToken cancellationToken)
    {
        var id = request.id;
        _ = await TimeEntryRepository.GetByIdAsync(id)
         ?? throw new NotFoundException("Time entry not found");
        await TimeEntryRepository.DeleteAsync(id);
    }
}