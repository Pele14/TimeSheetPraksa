using MediatR;
using TimeSheet.Application.Requests.TeamMemberRequests;

namespace TimeSheet.Application.TeamMember.Commands.UpdateTeamMember;


public record UpdateTeamMemberCommand(Guid Id, UpdateTeamMemberRequest Request) : IRequest;