

using Microsoft.EntityFrameworkCore;
using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;
using TimeSheet.Core.Repositories;
using TimeSheet.Infrastructure.Mappers;
using TimeSheet.Infrastructure.Persistence;

namespace TimeSheet.Infrastructure.Repositories;

public class CategoryRepository: ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context=context;

    }

    public async Task<List<CategoryModel>> GetAllAsync()
    {
        var entities= await _context.Categories
            .OrderBy(c=> c.Name)
            .ToListAsync();

        return entities.Select(c=>c.MapToDomain()).ToList();
    }

    public async Task<CategoryModel?> GetByIdAsync(Guid id)
    {
        var entity= await _context.Categories.FindAsync(id);
        return entity?.MapToDomain();
    }
}