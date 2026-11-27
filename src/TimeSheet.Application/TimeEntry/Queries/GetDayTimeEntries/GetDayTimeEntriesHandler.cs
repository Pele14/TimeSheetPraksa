using MediatR;
using TimeSheet.Application.Responses.TimeEntryResponse;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.TimeEntry.Queries.GetDayTimeEntries;



public class GetDayTimeEntriesHandler: IRequestHandler<GetDayTimeEntriesQuery, DayTimeEntriesResponse>
{
    private readonly ITimeEntryRepository TimeEntryRepository;
    private readonly IProjectRepository ProjectRepository;
    private readonly IClientRepository ClientRepository;
    private readonly ICategoryRepository CategoryRepository;
    private readonly ICurrentUserService CurrentUserService;

    public GetDayTimeEntriesHandler(
        ITimeEntryRepository timeEntryRepository,
        IProjectRepository projectRepository,
        IClientRepository clientRepository,
        ICategoryRepository categoryRepository,
        ICurrentUserService currentUserService)
    {
        TimeEntryRepository=timeEntryRepository;
        ProjectRepository=projectRepository;
        ClientRepository=clientRepository;
        CategoryRepository=categoryRepository;
        CurrentUserService=currentUserService;

    }

    public async Task<DayTimeEntriesResponse> Handle(GetDayTimeEntriesQuery request, CancellationToken cancellationToken)
    {
        var entries=await TimeEntryRepository.GetByMemberAndDateRangeAsync(CurrentUserService.UserId, request.Date, request.Date);
        var projects=(await ProjectRepository.GetAllAsync(includeInactive: true)).ToDictionary(p=>p.Id);
        var clients=(await ClientRepository.GetAllAsync()).ToDictionary(c=>c.Id);
        var categories=(await CategoryRepository.GetAllAsync()).ToDictionary(c=> c.Id);

        var responses=entries
                .Select(entry=>
                {
                    var project=projects[entry.ProjectId];
                    return entry.ToResponse(project, clients[project.ClientId], categories[entry.CategoryId]);

                })
                .ToList();
        return new DayTimeEntriesResponse
        {
            Date=request.Date,
            TotalHours=entries.Sum(e=>e.Hours + (e.Overtime ?? 0)),
            TimeEntries=responses
        };
    }
}