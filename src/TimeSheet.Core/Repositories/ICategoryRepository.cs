using Timesheet.Domain.Entities;

namespace TimeSheet.Core.Repositories;

public interface ICategoryRepository
{
    Task<CategoryModel?> GetByIdAsync(Guid id);
    Task<List<CategoryModel>> GetAllAsync();

}