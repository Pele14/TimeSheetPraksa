
using MediatR;
using TimeSheet.Application.TeamMember.DTOs;

namespace TimeSheet.Application.TeamMember.Commands.CreateTeamMember;


public record CreateTeamMemberCommand(CreateTeamMemberDTO dto) : IRequest<TeamMemberDTO>;

