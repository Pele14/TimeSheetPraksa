using TimeSheet.Infrastructure.Persistence;
using TimeSheet.Core.Repositories;
using Timesheet.Domain.Entities;
using TimeSheet.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
namespace TimeSheet.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectEntity?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Projects.FindAsync(id);
        return entity?.MapToDomain();
    }

    public async Task<IEnumerable<ProjectEntity>> GetAllAsync()
    {
        var entities = await _context.Projects.OrderBy(c=>c.Name).ToListAsync();
        return entities.Select(e => e.MapToDomain()).ToList();
    }

    public async Task AddAsync(ProjectEntity Project)
    {
        await _context.Projects.AddAsync(Project.MapToEntity());
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProjectEntity Project)
    {
        var entity = await _context.Projects.FindAsync(Project.Id)
          ?? throw new KeyNotFoundException($"Project {Project.Id} ne postoji.");

        Project.UpdateEntity(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var Project = await _context.Projects.FindAsync(id);
        if (Project != null)
        {
            _context.Projects.Remove(Project);
            await _context.SaveChangesAsync();
        }
        else
            throw new KeyNotFoundException("Id ne posoji");
    }
    public async Task<bool> ExistsByNameAsync(string name)
    {
        var normalized = name.Trim().ToLower();
        return await _context.Projects.AnyAsync(p => p.Name.ToLower() == normalized);


    }
    public async Task<bool> HasTimeEntriesAsync(Guid Projectid)
    {
        return await _context.TimeEntries.AnyAsync(te=>te.ProjectId==Projectid);    
    }
}