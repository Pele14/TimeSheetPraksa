
using MediatR;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.TeamMember.Commands.DeleteTeamMember;


public class DeleteTeamMemberHandler : IRequestHandler<DeleteTeamMemberCommand>
{
    private readonly ITeamMemberRepository TeamMemberRepository;

    public DeleteTeamMemberHandler(ITeamMemberRepository teamMemberRepository)
    {
        TeamMemberRepository = teamMemberRepository;
    }



    public async Task Handle(DeleteTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var id = request.id;
        var entity = await TeamMemberRepository.GetByIdAsync(id) ?? throw new NotFoundException("Email doesn't exist");
        await TeamMemberRepository.DeleteAsync(id);



    }
}