using Timesheet.Domain.Entities;


namespace TimeSheet.Core.Repositories;

public interface IProjectRepository
{
    Task<ProjectEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProjectEntity>> GetAllAsync();
    Task AddAsync(ProjectEntity ProjectEntity);
    Task UpdateAsync(ProjectEntity ProjectEntity);
    Task DeleteAsync(Guid id);

    Task<bool> HasTimeEntriesAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);

}