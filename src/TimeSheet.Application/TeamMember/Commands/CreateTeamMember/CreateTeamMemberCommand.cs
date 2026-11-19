
using MediatR;
using TimeSheet.Application.Requests.TeamMemberRequests;
using TimeSheet.Application.Responses.TeamMemberResponses;


namespace TimeSheet.Application.TeamMember.Commands.CreateTeamMember;


public record CreateTeamMemberCommand(CreateTeamMemberRequest Request) : IRequest<TeamMemberResponse>;

