using MediatR;
using TimeSheet.Application.TeamMember.DTOs;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.TeamMember.Queries.GetTeamMembers;



public class GetTeamMembersHandler : IRequestHandler<GetTeamMembersQuery, List<TeamMemberDTO>>
{
    private readonly ITeamMemberRepository TeamMemberRepository;

    public GetTeamMembersHandler(ITeamMemberRepository teamMemberRepository)
    {
        TeamMemberRepository = teamMemberRepository;
    }
    public async Task<List<TeamMemberDTO>> Handle(GetTeamMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await TeamMemberRepository.GetAllAsync();

        return members.Select(m => m.ToDTO()).ToList();
    }
}