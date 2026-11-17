using Microsoft.EntityFrameworkCore;
using Timesheet.Domain.Entities;
using TimeSheet.Infrastructure.Persistence;
using TimeSheet.Domain.Interfaces;
using TimeSheet.Infrastructure.Mappers;


namespace TimeSheet.Infrastructure.Repositories;

public class TeamMemberRepository : ITeamMemberRepository
{
    private readonly AppDbContext _context;

    public TeamMemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TeamMemberEntity?> GetByIdAsync(Guid id)
    {
       var entity= await _context.TeamMembers.FindAsync();
       return entity?.MapToDomain();
    }

    public async Task<IEnumerable<TeamMemberEntity>> GetAllAsync()
    {
        var entities= await _context.TeamMembers.ToListAsync();
        return entities.Select(e=> e.MapToDomain()).ToList();
    }

    public async Task AddAsync(TeamMemberEntity teamMember)
    {
        await _context.TeamMembers.AddAsync(teamMember.MapToEntity());
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TeamMemberEntity teamMember)
    {
        _context.TeamMembers.Update(teamMember.MapToEntity());
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
    public async Task<TeamMemberEntity?> GetByEmailAsync(string email)
    {   
        var normalized=email.Trim().ToLowerInvariant();
        var entity=await _context.TeamMembers.FirstOrDefaultAsync(t=> t.Email==normalized);

        return entity?.MapToDomain();

    }
}

