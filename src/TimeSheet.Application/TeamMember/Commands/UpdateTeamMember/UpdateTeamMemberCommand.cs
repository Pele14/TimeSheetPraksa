using MediatR;
using TimeSheet.Application.TeamMember.DTOs;

namespace TimeSheet.Application.TeamMember.Commands.UpdateTeamMember;


public record UpdateTeamMemberCommand(Guid Id, UpdateTeamMemberDto Dto) : IRequest;