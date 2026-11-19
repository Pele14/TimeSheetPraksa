
using MediatR;
using TimeSheet.Application.TeamMember.DTOs;

namespace TimeSheet.Application.TeamMember.Queries.GetTeamMemberById;


public record GetTeamMemberByIdQuery(Guid id) : IRequest<TeamMemberDTO>;