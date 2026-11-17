
using TimeSheet.Application.TeamMember.DTOs;    

namespace TimeSheet.Application.TeamMember;

    public interface ITeamMemberService
    {
        Task<List<TeamMemberDTO>>GetAllAsync();
        Task<TeamMemberDTO?>GetByIdAsync(Guid id);

        Task<TeamMemberDTO>CreateAsync(CreateTeamMemberDTO dto);

        Task UpdateAsync(Guid id, CreateTeamMemberDTO dto);

        Task DeleteAsync(Guid id);

      

    }
