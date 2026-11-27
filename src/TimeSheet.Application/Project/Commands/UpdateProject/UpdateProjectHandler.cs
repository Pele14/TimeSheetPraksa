using MediatR;
using TimeSheet.Application.Common.Exceptions;

using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.Project.Commands.UpdateProject;


public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IProjectRepository ProjectRepository;
    private readonly IClientRepository ClientRepository;
    private readonly ITeamMemberRepository TeamMemberRepository;
    public UpdateProjectHandler(IProjectRepository projectRepository, IClientRepository clientRepository, ITeamMemberRepository teamMemberRepository)
    {
        ProjectRepository = projectRepository;
        ClientRepository= clientRepository;
        TeamMemberRepository= teamMemberRepository;
    }

    public async Task Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        var request=command.Request;
        var entity = await ProjectRepository.GetByIdAsync(command.id, includeInactive: true) ??
            throw new NotFoundException("Project doesnt exist");
        var namechanged=!string.Equals(entity.Name, request.Name.Trim(), StringComparison.OrdinalIgnoreCase);
        if (namechanged && await ProjectRepository.ExistsByNameAsync(request.Name))
            throw new ConflictException("Project name is already in use");
        _= await ClientRepository.GetByIdAsync(request.ClientId)??
            throw new NotFoundException("Client doesnt exist");
        _= await TeamMemberRepository.GetByIdAsync(request.LeadID)??
            throw new NotFoundException("Lead doesnt exist");

        entity.Update(request.Name, request.Description, request.Status, request.ClientId, request.LeadID);
        await ProjectRepository.UpdateAsync(entity);
    }
}