using MediatR;
using TimeSheet.Application.Project.Queries.GetProjects;
using TimeSheet.Application.Responses.ProjectResponses;
using TimeSheet.Application.Responses.TeamMemberResponses;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.Project.Queries.GetProjects;



public class GetProjectsHandler : IRequestHandler<GetProjectsQuery, ProjectsResponse>
{
    private readonly IProjectRepository ProjectRepository;
    private readonly IClientRepository ClientRepository;
    public GetProjectsHandler(IProjectRepository projectRepository, IClientRepository clientRepository)
    {
        ProjectRepository = projectRepository;
        ClientRepository=clientRepository;
    }
    public async Task<ProjectsResponse> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await ProjectRepository.GetAllAsync();
        var clients= await ClientRepository.GetAllAsync();
        var clientName=clients.ToDictionary(c=>c.Id, c=>c.Name);
        return new ProjectsResponse
        {
           
           Projects= projects.Select(p => p.ToResponse(clientName.GetValueOrDefault
            (p.ClientId, string.Empty))).ToList()


        };
    }
}