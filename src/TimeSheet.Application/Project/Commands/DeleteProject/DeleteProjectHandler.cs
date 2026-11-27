
using MediatR;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.Project.Commands.DeleteProject;


public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IProjectRepository ProjectRepository;

    public DeleteProjectHandler(IProjectRepository projectRepository)
    {
        ProjectRepository = projectRepository;
    }



    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id;
        _ = await ProjectRepository.GetByIdAsync(id, includeInactive: true) 
            ?? throw new NotFoundException("Project doesn't exist");
        await ProjectRepository.DeleteAsync(id);

    }
}