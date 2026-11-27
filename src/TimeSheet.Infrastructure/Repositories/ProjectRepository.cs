using TimeSheet.Infrastructure.Persistence;
using TimeSheet.Core.Repositories;
using Timesheet.Domain.Entities;
using TimeSheet.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
using Timesheet.Infrastructure.DBEntities;
using TimeSheet.Infrastructure.Configurations;
using System.Linq.Expressions;
namespace TimeSheet.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }
    private IQueryable<Project> Projects(bool includeInactive)=>
        includeInactive
            ? _context.Projects.IgnoreQueryFilters([ProjectFilters.ActiveOnly])
            : _context.Projects;

    public async Task<ProjectModel?> GetByIdAsync(Guid id,bool includeInactive=false)
    {
        var entity = await Projects(includeInactive).FirstOrDefaultAsync(p=> p.Id==id);
        return entity?.MapToDomain();
    }

    public async Task<IEnumerable<ProjectModel>> GetAllAsync(bool includeInactive=false)
    {
        var entities = await Projects(includeInactive)
                .OrderBy(p=> p.Name)
                .ToListAsync();
        return entities.Select(e => e.MapToDomain()).ToList();
    }

    public async Task AddAsync(ProjectModel Project)
    {
        await _context.Projects.AddAsync(Project.MapToEntity());
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProjectModel project)
    {
        var entity = await Projects(includeInactive: true).FirstOrDefaultAsync(p=> p.Id==project.Id)
          ?? throw new KeyNotFoundException($"Project {project.Id} doesnt exist.");

        project.UpdateEntity(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var Project = await Projects(includeInactive: true).FirstOrDefaultAsync(p=> p.Id==id);
        if (Project != null)
        {
            var timEntries=await _context.TimeEntries
                    .Where(te=>te.ProjectId==id)
                    .ToListAsync();
            _context.TimeEntries.RemoveRange(timEntries);        
            _context.Projects.Remove(Project);
            await _context.SaveChangesAsync();
        }
        else
            throw new KeyNotFoundException("Id doesnt exist");
    }
    public async Task<bool> ExistsByNameAsync(string name)
    {
        var normalized = name.Trim().ToLower();
        return await Projects(includeInactive: true).AnyAsync(p => p.Name.ToLower() == normalized);


    }

}