
using MediatR;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.TimeEntry.Commands.DeleteTimeEntry;


public class DeleteTimeEntryHandler : IRequestHandler<DeleteTimeEntryCommand>
{
    private readonly ITimeEntryRepository TimeEntryRepository;
    private readonly ICurrentUserService CurrentUserService;

    public DeleteTimeEntryHandler(ITimeEntryRepository timeEntryRepository, ICurrentUserService currentUserService)
    {
        TimeEntryRepository = timeEntryRepository;
        CurrentUserService=currentUserService;
    }



    public async Task Handle(DeleteTimeEntryCommand request, CancellationToken cancellationToken)
    {
        var id = request.id;
        var timeEntry= await TimeEntryRepository.GetByIdAsync(id)
         ?? throw new NotFoundException("Time entry not found");
        if(timeEntry.TeamMemberId!= CurrentUserService.UserId)
            throw new ForbiddenException("You can only delete your own time entities");

        await TimeEntryRepository.DeleteAsync(id);
    }
}