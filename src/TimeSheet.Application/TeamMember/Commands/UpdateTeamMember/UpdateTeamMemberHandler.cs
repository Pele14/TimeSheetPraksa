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

    public async Task Handle(UpdateTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var entity = await TeamMemberRepository.GetByIdAsync(request.Id) ??
        throw new NotFoundException("TeamMember doesnt exist");

        var dto = request.Dto;
        var email = dto.Email.Trim().ToLowerInvariant();
        if (entity.Email != email && await TeamMemberRepository.ExistsByEmailAsync(email))
            throw new ConflictException("Email is already in use");
        entity.Update(dto.Name, dto.Username, dto.Email, dto.HoursPerWeek, dto.Role, dto.Status);
        await TeamMemberRepository.UpdateAsync(entity);
    }
}