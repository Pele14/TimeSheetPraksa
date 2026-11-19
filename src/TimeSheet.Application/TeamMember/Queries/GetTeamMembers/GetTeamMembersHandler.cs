using MediatR;
using TimeSheet.Application.Responses.TeamMemberResponses;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.TeamMember.Queries.GetTeamMembers;



public class GetTeamMembersHandler : IRequestHandler<GetTeamMembersQuery, TeamMembersResponse>
{
    private readonly ITeamMemberRepository TeamMemberRepository;

    public GetTeamMembersHandler(ITeamMemberRepository teamMemberRepository)
    {
        TeamMemberRepository = teamMemberRepository;
    }
    public async Task<TeamMembersResponse> Handle(GetTeamMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await TeamMemberRepository.GetAllAsync();

        return new TeamMembersResponse
        {
           TeamMembers= members.Select(m => m.ToResponse()).ToList()


        };
    }
}