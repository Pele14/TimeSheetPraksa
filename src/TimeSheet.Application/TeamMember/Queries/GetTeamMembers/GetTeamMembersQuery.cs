using MediatR;
using TimeSheet.Application.TeamMember.DTOs;

namespace TimeSheet.Application.TeamMember.Queries.GetTeamMembers;


public record GetTeamMembersQuery(): IRequest<List<TeamMemberDTO>>;