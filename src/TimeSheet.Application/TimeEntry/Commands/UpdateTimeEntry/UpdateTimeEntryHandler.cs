
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
   

    public UpdateTimeEntryHandler(
        ITimeEntryRepository timeEntryRepository,
        IProjectRepository projectRepository,
        ICategoryRepository categoryRepository)
    {
        TimeEntryRepository = timeEntryRepository;
        ProjectRepository=projectRepository;
        CategoryRepository=categoryRepository;
    }


    public async Task Handle(UpdateTimeEntryCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var timeEntry=await TimeEntryRepository.GetByIdAsync(command.Id)
                    ?? throw new NotFoundException("Time entry not found");
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
            projectId: request.CategoryId,
            categoryId: request.ProjectId);
        
        
        await TimeEntryRepository.UpdateAsync(timeEntry);
    }
}