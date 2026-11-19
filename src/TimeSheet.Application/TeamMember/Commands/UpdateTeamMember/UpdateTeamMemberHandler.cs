using MediatR;
using TimeSheet.Application.Common.Exceptions;

using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.TeamMember.Commands.UpdateTeamMember;


public class UpdateTeamMemberHandler : IRequestHandler<UpdateTeamMemberCommand>
{
    private readonly ITeamMemberRepository TeamMemberRepository;

    public UpdateTeamMemberHandler(ITeamMemberRepository teamMemberRepository)
    {
        TeamMemberRepository = teamMemberRepository;

    }

    public async Task Handle(UpdateTeamMemberCommand command, CancellationToken cancellationToken)
    {
        var entity = await TeamMemberRepository.GetByIdAsync(command.Id) ??
        throw new NotFoundException("TeamMember doesnt exist");

        var request = command.Request;
        var email = request.Email.Trim().ToLowerInvariant();
        if (entity.Email != email && await TeamMemberRepository.ExistsByEmailAsync(email))
            throw new ConflictException("Email is already in use");
        entity.Update(request.Name, request.Username, request.Email, request.HoursPerWeek, request.Role, request.Status);
        await TeamMemberRepository.UpdateAsync(entity);
    }
}