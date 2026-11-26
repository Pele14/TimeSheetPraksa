
using System.Diagnostics;
using MediatR;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Responses.TimeEntryResponse;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.TimeEntry.Commands.UpdateTimeEntry;


public class UpdateTimeEntryHandler : IRequestHandler<UpdateTimeEntryCommand>
{
    private readonly ITimeEntryRepository TimeEntryRepository;
    private readonly IProjectRepository ProjectRepository;
    private readonly ICategoryRepository CategoryRepository;
    private readonly ICurrentUserService CurrentUserService;
   

    public UpdateTimeEntryHandler(
        ITimeEntryRepository timeEntryRepository,
        IProjectRepository projectRepository,
        ICategoryRepository categoryRepository,
        ICurrentUserService currentUserService)
    {
        TimeEntryRepository = timeEntryRepository;
        ProjectRepository=projectRepository;
        CategoryRepository=categoryRepository;
        CurrentUserService=currentUserService;
    }


    public async Task Handle(UpdateTimeEntryCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var timeEntry=await TimeEntryRepository.GetByIdAsync(command.Id)
                    ?? throw new NotFoundException("Time entry not found");
        if(timeEntry.TeamMemberId!= CurrentUserService.UserId)
                throw new ForbiddenException("You can only modify your own entries");
        _=await ProjectRepository.GetByIdAsync(request.ProjectId)
                    ?? throw new NotFoundException("Project not found");
        _=await CategoryRepository.GetByIdAsync(request.CategoryId)
                    ?? throw new NotFoundException("TeamMember not found");

        timeEntry.Update
        (
            request.Date,
            request.Hours,
            request.Overtime,
            request.Description,
            categoryId: request.CategoryId,
            projectId: request.ProjectId);
        
        
        await TimeEntryRepository.UpdateAsync(timeEntry);
    }
}