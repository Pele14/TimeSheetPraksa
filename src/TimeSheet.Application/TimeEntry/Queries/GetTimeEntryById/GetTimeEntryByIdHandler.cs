using MediatR;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Responses.TimeEntryResponse;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.TimeEntry.Queries.GetTimeEntryById;


public class GetTimeEntryByIdHandler : IRequestHandler<GetTimeEntryByIdQuery, TimeEntryResponse>
{
    private readonly ITimeEntryRepository TimeEntryRepository;
    private readonly IProjectRepository ProjectRepository;
    private readonly IClientRepository ClientRepository;
    private readonly ICategoryRepository CategoryRepository;
    public GetTimeEntryByIdHandler(
        ITimeEntryRepository timeEntryRepository,
        IProjectRepository projectRepository,
        IClientRepository clientRepository,
        ICategoryRepository categoryRepository)
    {
        TimeEntryRepository = timeEntryRepository;
        ProjectRepository=projectRepository;
        ClientRepository=clientRepository;
        CategoryRepository=categoryRepository;

    }



    public async Task<TimeEntryResponse> Handle(GetTimeEntryByIdQuery request, CancellationToken cancellationToken)
    {
        var timeEntry=await TimeEntryRepository.GetByIdAsync(request.id)
                    ?? throw new NotFoundException("Time entry not found");
        var project=await ProjectRepository.GetByIdAsync(timeEntry.ProjectId)
                    ?? throw new NotFoundException("Project not found");
        var category=await CategoryRepository.GetByIdAsync(timeEntry.CategoryId)
                    ?? throw new NotFoundException("Category not found");
        var client=await ClientRepository.GetByIdAsync(project.ClientId)
                    ?? throw new NotFoundException("Project not found");
        
        return timeEntry.ToResponse(project, client, category);
    }

}
