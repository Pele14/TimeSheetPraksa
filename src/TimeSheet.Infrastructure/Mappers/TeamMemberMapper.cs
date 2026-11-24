using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Mappers;



public static class TeamMemberMapper
{
    public static TeamMember MapToEntity(this TeamMemberModel TeamMember)

        => new TeamMember
        {
            Id = TeamMember.Id,
            Name = TeamMember.Name,
            Username = TeamMember.Username,
            Email = TeamMember.Email,
            PasswordHash = TeamMember.PasswordHash,
            HoursPerWeek = TeamMember.HoursPerWeek,
            Role = TeamMember.Role,
            Status = TeamMember.Status

        };



    public static TeamMemberModel MapToDomain(this TeamMember TeamMember)

        => new TeamMemberModel(TeamMember.Id, TeamMember.Name, TeamMember.Username, TeamMember.Email, TeamMember.PasswordHash, TeamMember.HoursPerWeek, TeamMember.Role, TeamMember.Status);

    public static void UpdateEntity(this TeamMemberModel entity, TeamMember TeamMember)
    {
        TeamMember.Name = entity.Name;
        TeamMember.Username = entity.Username;
        TeamMember.Email = entity.Email;
        TeamMember.HoursPerWeek = entity.HoursPerWeek;
        TeamMember.Role = entity.Role;
        TeamMember.Status = entity.Status;

    }


}