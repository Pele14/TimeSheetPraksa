using Microsoft.EntityFrameworkCore;
using Timesheet.Domain.Entities;
using TimeSheet.Domain.Interfaces;
using TimeSheet.Infrastructure.Persistence;

namespace TimeSheet.Infrastructure.Repositories;
/*
public class TeamMemberRepository : ITeamMemberRepository
{
    private readonly AppDbContext _context;

    public TeamMemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TeamMember> GetByIdAsync(Guid id)
    {
        if(await _context.TeamMembers.FindAsync(id) is TeamMember teamMember)
        {
            return teamMember;
        }

        throw new InvalidOperationException("Team member not found.");
    }

    public async Task<IEnumerable<TeamMember>> GetAllAsync()
    {
        return await _context.TeamMembers.ToListAsync();
    }

    public async Task AddAsync(TeamMember teamMember)
    {
        await _context.TeamMembers.AddAsync(teamMember);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TeamMember teamMember)
    {
        _context.TeamMembers.Update(teamMember);
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
    }
}*/