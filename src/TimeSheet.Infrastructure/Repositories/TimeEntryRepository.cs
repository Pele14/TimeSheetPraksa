using TimeSheet.Infrastructure.Persistence;
using TimeSheet.Core.Repositories;
using Timesheet.Domain.Entities;
using TimeSheet.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
namespace TimeSheet.Infrastructure.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly AppDbContext _context;

    public TimeEntryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TimeEntryModel?> GetByIdAsync(Guid id)
    {
        var entity = await _context.TimeEntries.FindAsync(id);
        return entity?.MapToDomain();
    }
    public async Task AddAsync(TimeEntryModel TimeEntry)
    {
        await _context.TimeEntries.AddAsync(TimeEntry.MapToEntity());
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TimeEntryModel TimeEntry)
    {
        var entity = await _context.TimeEntries.FindAsync(TimeEntry.Id)
          ?? throw new KeyNotFoundException($"Time Entry {TimeEntry.Id} doesnt exist.");

        TimeEntry.UpdateEntity(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var TimeEntry = await _context.TimeEntries.FindAsync(id);
        if (TimeEntry != null)
        {
            _context.TimeEntries.Remove(TimeEntry);
            await _context.SaveChangesAsync();
        }
        else
            throw new KeyNotFoundException("Id doesnt exist");
    }

    public async Task<List<TimeEntryModel>> GetByMemberAndDateRangeAsync(Guid teammemberId, DateOnly from, DateOnly to)
    {
        var entities=await _context.TimeEntries
            .Where(te=>te.TeamMemberId==teammemberId && te.Date>=from && te.Date<=to)
            .OrderBy(te=> te.Date)
            .ThenBy(te=> te.CreatedAt)
            .ToListAsync();
        
        return entities.Select(te=> te.MapToDomain()).ToList();
    }
}