using Microsoft.EntityFrameworkCore;
using Timesheet.Domain.Entities;
using TimeSheet.Infrastructure.Persistence;
using TimeSheet.Core.Repositories;
using TimeSheet.Infrastructure.Mappers;


namespace TimeSheet.Infrastructure.Repositories;

public class TeamMemberRepository : ITeamMemberRepository
{
    private readonly AppDbContext _context;

    public TeamMemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TeamMemberModel?> GetByIdAsync(Guid id)
    {
        var entity = await _context.TeamMembers.FindAsync(id);
        return entity?.MapToDomain();
    }

    public async Task<IEnumerable<TeamMemberModel>> GetAllAsync()
    {
        var entities = await _context.TeamMembers.ToListAsync();
        return entities.Select(e => e.MapToDomain()).ToList();
    }

    public async Task AddAsync(TeamMemberModel teamMember)
    {
        await _context.TeamMembers.AddAsync(teamMember.MapToEntity());
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TeamMemberModel teamMember)
    {
        var entity = await _context.TeamMembers.FindAsync(teamMember.Id)
          ?? throw new KeyNotFoundException($"Član tima {teamMember.Id} ne postoji.");

        teamMember.UpdateEntity(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var teamMember = await _context.TeamMembers.FindAsync(id);
        if (teamMember != null)
        {
            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();
        }
        else
            throw new KeyNotFoundException("Id ne posoji");
    }
    public async Task<TeamMemberModel?> GetByEmailAsync(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();
        var entity = await _context.TeamMembers.FirstOrDefaultAsync(t => t.Email == normalized);

        return entity?.MapToDomain();

    }
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        var normalized = email.Trim().ToLowerInvariant();

        return await _context.TeamMembers
        .AnyAsync(t => t.Email.ToLower() == normalized);
    }
}

