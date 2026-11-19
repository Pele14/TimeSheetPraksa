using MediatR;


namespace TimeSheet.Application.TeamMember.Commands.DeleteTeamMember;

public record DeleteTeamMemberCommand(Guid id) : IRequest;