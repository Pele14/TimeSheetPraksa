using Timesheet.Domain.Entities;


namespace TimeSheet.Core.Repositories;

public interface IProjectRepository
{
    Task<ProjectModel?> GetByIdAsync(Guid id, bool includeInactive=false);
    Task<IEnumerable<ProjectModel>> GetAllAsync(bool includeInactive=false);
    Task AddAsync(ProjectModel ProjectEntity);
    Task UpdateAsync(ProjectModel ProjectEntity);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);

}