using MediatR;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Responses.TeamMemberResponses;
using TimeSheet.Application.TeamMember.Queries.GetTeamMembers;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.TeamMember.Queries.GetTeamMemberById;


public class GetTeamMemberByIdHandler : IRequestHandler<GetTeamMemberByIdQuery, TeamMemberResponse>
{
    private readonly ITeamMemberRepository TeamMemberRepository;

    public GetTeamMemberByIdHandler(ITeamMemberRepository teamMemberRepository)
    {
        TeamMemberRepository = teamMemberRepository;

    }



    public async Task<TeamMemberResponse> Handle(GetTeamMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var Id = request.id;

        var entity = await TeamMemberRepository.GetByIdAsync(Id) ??
        throw new NotFoundException("Entity doesnt exist");


        return entity.ToResponse();

    }

    public static implicit operator GetTeamMemberByIdHandler(GetTeamMembersHandler v)
    {
        throw new NotImplementedException();
    }
}
