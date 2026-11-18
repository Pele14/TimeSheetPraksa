using Timesheet.Domain.Entities;

namespace TimeSheet.Application.TeamMember.DTOs;


public static class TeamMemberDtoMapper
{
    public static TeamMemberDTO ToDTO(this TeamMemberEntity member) //extension metoda na TeamMemberEntity
    {
        return new TeamMemberDTO(member.Id, member.Name, member.Username,
        member.Email, member.HoursPerWeek, member.Role, member.Status);



    }

}
