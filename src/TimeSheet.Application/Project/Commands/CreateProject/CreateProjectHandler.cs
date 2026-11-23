
using System.IO.Compression;
using MediatR;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.ProjectResponses;
using TimeSheet.Application.Responses.ProjectResponses;
using TimeSheet.Application.Responses.TeamMemberResponses;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.Project.Commands.CreateProject;


public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ProjectResponse>
{
    private readonly IProjectRepository ProjectRepository;
    private readonly IClientRepository ClientRepository;
    private readonly ITeamMemberRepository TeamMemberRepository;

    public CreateProjectHandler(IProjectRepository projectRepository, IClientRepository clientRepository, ITeamMemberRepository teamMemberRepository)
    {
        ProjectRepository = projectRepository;
        ClientRepository=clientRepository;
        TeamMemberRepository=teamMemberRepository;

    }


    public async Task<ProjectResponse> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var name = request.Name;
        if (await ProjectRepository.ExistsByNameAsync(name))
        {
            throw new ConflictException("Name already exists");
        }
        var client=await ClientRepository.GetByIdAsync(request.ClientId)??
            throw new NotFoundException("Client not found");
        
        _= await TeamMemberRepository.GetByIdAsync(request.LeadID)??
            throw new NotFoundException("Lead not found"); 

        var project = new ProjectEntity(Guid.NewGuid(), request.Name,
             request.Description, ProjectStatus.Active,
             request.ClientId, request.LeadID);

        await ProjectRepository.AddAsync(project);


        return project.ToResponse(client.Name);
    }

}