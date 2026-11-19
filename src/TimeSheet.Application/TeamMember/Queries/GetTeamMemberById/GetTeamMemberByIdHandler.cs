using MediatR;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.TeamMember.DTOs;
using TimeSheet.Application.TeamMember.Queries.GetTeamMembers;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.TeamMember.Queries.GetTeamMemberById;


public class GetTeamMemberByIdHandler : IRequestHandler<GetTeamMemberByIdQuery, TeamMemberDTO>
{
    private readonly ITeamMemberRepository TeamMemberRepository;

    public GetTeamMemberByIdHandler(ITeamMemberRepository teamMemberRepository)
    {
        TeamMemberRepository = teamMemberRepository;

    }



    public async Task<TeamMemberDTO> Handle(GetTeamMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var Id = request.id;

        var entity = await TeamMemberRepository.GetByIdAsync(Id) ??
        throw new NotFoundException("Entity doesnt exist");

        var dto = entity.ToDTO();

        return dto;


    }

    public static implicit operator GetTeamMemberByIdHandler(GetTeamMembersHandler v)
    {
        throw new NotImplementedException();
    }
}
