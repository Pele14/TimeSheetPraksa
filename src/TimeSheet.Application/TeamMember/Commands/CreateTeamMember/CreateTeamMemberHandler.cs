
using MediatR;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Responses.TeamMemberResponses;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;

namespace TimeSheet.Application.TeamMember.Commands.CreateTeamMember;


public class CreateTeamMemberHandler : IRequestHandler<CreateTeamMemberCommand, TeamMemberResponse>
{
    private readonly ITeamMemberRepository TeamMemberRepository;
    private readonly IPasswordHasher PasswordHasher;

    public CreateTeamMemberHandler(ITeamMemberRepository teamMemberRepository, IPasswordHasher passwordHasher)
    {
        TeamMemberRepository = teamMemberRepository;
        PasswordHasher = passwordHasher;
    }


    public async Task<TeamMemberResponse> Handle(CreateTeamMemberCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var email = request.Email;
        if (await TeamMemberRepository.ExistsByEmailAsync(email))
        {
            throw new ConflictException("Email already exists");
        }
        var password = PasswordHasher.Hash(request.Password);

        var member = new TeamMemberEntity(Guid.NewGuid(), request.Name,
            request.Username,
             request.Email, password,
             request.HoursPerWeek, request.Role,
             request.Status);

        await TeamMemberRepository.AddAsync(member);


        return member.ToResponse();
    }
}