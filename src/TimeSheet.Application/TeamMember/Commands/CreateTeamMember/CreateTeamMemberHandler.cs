
using MediatR;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.TeamMember.DTOs;
using TimeSheet.Domain.Interfaces;

namespace TimeSheet.Application.TeamMember.Commands.CreateTeamMember;


public class CreateTeamMemberHandler : IRequestHandler<CreateTeamMemberCommand, TeamMemberDTO>
{
    private readonly ITeamMemberRepository TeamMemberRepository;
    private readonly IPasswordHasher PasswordHasher;

    public CreateTeamMemberHandler(ITeamMemberRepository teamMemberRepository, IPasswordHasher passwordHasher)
    {
        TeamMemberRepository = teamMemberRepository;
        PasswordHasher = passwordHasher;
    }


    public async Task<TeamMemberDTO> Handle(CreateTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var dto = request.dto;
        var email = dto.Email;
        if (await TeamMemberRepository.ExistsByEmailAsync(email))
        {
            throw new ConflictException("Email already exists");
        }
        var password = PasswordHasher.Hash(dto.PasswordHash);

        var member = new TeamMemberEntity(Guid.NewGuid(), dto.Name,
            dto.Username,
             dto.Email, password,
             dto.HoursPerWeek, dto.Role,
             dto.Status);

        await TeamMemberRepository.AddAsync(member);


        return member.ToDTO();
    }
}