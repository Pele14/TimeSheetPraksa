
using MediatR;
using TimeSheet.Application.Responses.TeamMemberResponses;

namespace TimeSheet.Application.TeamMember.Queries.GetTeamMemberById;


public record GetTeamMemberByIdQuery(Guid id) : IRequest<TeamMemberResponse>;