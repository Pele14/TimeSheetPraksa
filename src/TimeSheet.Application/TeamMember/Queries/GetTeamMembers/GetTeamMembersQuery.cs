using MediatR;
using TimeSheet.Application.Responses.TeamMemberResponses;

namespace TimeSheet.Application.TeamMember.Queries.GetTeamMembers;


public record GetTeamMembersQuery() : IRequest<TeamMembersResponse>;