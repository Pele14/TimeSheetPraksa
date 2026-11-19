using Timesheet.Domain.Entities;
using TimeSheet.Application.Responses.TeamMemberResponses;

public static class TeamMemberDtoMapper
{
    public static TeamMemberResponse ToResponse(this TeamMemberEntity member) //extension metoda na TeamMemberEntity
    {
        return new TeamMemberResponse
        {
            Id=member.Id,
            Name=member.Name,
            Username=member.Username,
            Email=member.Email,
            HoursPerWeek=member.HoursPerWeek,
            Role=member.Role,
            Status=member.Status

        };

    }

}