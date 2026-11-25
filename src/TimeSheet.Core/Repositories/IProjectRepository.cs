using Timesheet.Domain.Entities;


namespace TimeSheet.Core.Repositories;

public interface IProjectRepository
{
    Task<ProjectModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProjectModel>> GetAllAsync();
    Task AddAsync(ProjectModel ProjectEntity);
    Task UpdateAsync(ProjectModel ProjectEntity);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);

}