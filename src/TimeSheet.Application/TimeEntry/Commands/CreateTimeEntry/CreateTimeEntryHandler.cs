
using System.Diagnostics;
using MediatR;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Responses.TimeEntryResponse;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.TimeEntry.Commands.CreateTimeEntry;


public class CreateTimeEntryHandler : IRequestHandler<CreateTimeEntryCommand, TimeEntryResponse>
{
    private readonly ITimeEntryRepository TimeEntryRepository;
    private readonly IProjectRepository ProjectRepository;
    private readonly IClientRepository ClientRepository;
    private readonly ICategoryRepository CategoryRepository;
    private readonly ITeamMemberRepository TeamMemberRepository;
    private readonly ICurrentUserService CurrentUserService;
    public CreateTimeEntryHandler(
        ITimeEntryRepository timeEntryRepository,
        IProjectRepository projectRepository,
        IClientRepository clientRepository,
        ICategoryRepository categoryRepository,
        ITeamMemberRepository teamMemberRepository,
        ICurrentUserService currentUserService)
    {
        TimeEntryRepository = timeEntryRepository;
        ProjectRepository=projectRepository;
        ClientRepository=clientRepository;
        CategoryRepository=categoryRepository;
        TeamMemberRepository=teamMemberRepository;
        CurrentUserService=currentUserService;

    }


    public async Task<TimeEntryResponse> Handle(CreateTimeEntryCommand command, CancellationToken cancellationToken)
    {
        var userId=CurrentUserService.UserId;
        var request = command.Request;
        var project=await ProjectRepository.GetByIdAsync(request.ProjectId)
                    ?? throw new NotFoundException("Project not found");
        var category=await CategoryRepository.GetByIdAsync(request.CategoryId)
                    ?? throw new NotFoundException("Category not found");
        _=await TeamMemberRepository.GetByIdAsync(userId)
                    ?? throw new NotFoundException("TeamMember not found");
        var client=await ClientRepository.GetByIdAsync(project.ClientId)
                    ?? throw new NotFoundException("Project not found");
        

        var timeEntry=new TimeEntryModel
        (
            Guid.NewGuid(),
            request.Date,
            request.Hours,
            request.Overtime,
            request.Description,
            category: request.CategoryId,
            project: request.ProjectId,
            teammember: userId);
        await TimeEntryRepository.AddAsync(timeEntry);


        return timeEntry.ToResponse(project, client, category);
    }
}