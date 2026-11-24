using MediatR;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Responses.ProjectResponses;
using TimeSheet.Core.Repositories;
using TimeSheet.Application.Requests.ProjectRequests;
using TimeSheet.Application.Requests.ProjectResponses;

namespace TimeSheet.Application.Project.Queries.GetProjectById;


public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectResponse>
{
    private readonly IProjectRepository ProjectRepository;
    private readonly IClientRepository ClientRepository;
    public GetProjectByIdHandler(IProjectRepository projectRepository, IClientRepository clientRepository)
    {
        ProjectRepository = projectRepository;
        ClientRepository=clientRepository;
    }



    public async Task<ProjectResponse> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var Id = request.Id;

        var entity = await ProjectRepository.GetByIdAsync(Id) ??
            throw new NotFoundException("Project doesnt exist");
        var client= await ClientRepository.GetByIdAsync(entity.ClientId);

        return entity.ToResponse(client?.Name ?? string.Empty);

    }

}


