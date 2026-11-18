
using Timesheet.Domain.Entities;


namespace TimeSheet.Domain.Interfaces;

    public interface ITeamMemberRepository
    {
        Task<TeamMemberEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TeamMemberEntity>> GetAllAsync();
        Task AddAsync(TeamMemberEntity EnTeamMemberEntity);
        Task UpdateAsync(TeamMemberEntity EnTeamMemberEntity);
        Task DeleteAsync(Guid id);

        Task<TeamMemberEntity?>GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);

    }