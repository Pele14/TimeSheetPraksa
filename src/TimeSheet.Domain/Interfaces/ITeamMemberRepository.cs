
using Timesheet.Domain.Entities;


namespace TimeSheet.Domain.Interfaces;

public interface ITeamMemberRepository
{
    Task<TeamMemberModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<TeamMemberModel>> GetAllAsync();
    Task AddAsync(TeamMemberModel EnTeamMemberEntity);
    Task UpdateAsync(TeamMemberModel EnTeamMemberEntity);
    Task DeleteAsync(Guid id);

    Task<TeamMemberModel?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);

}