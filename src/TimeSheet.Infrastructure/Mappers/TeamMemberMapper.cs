using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Mappers;



public static class TeamMemberMapper
{
    public static TeamMember MapToEntity(this TeamMemberEntity TeamMember)
    
        => new TeamMember
        {
            Id=TeamMember.Id,
            Name=TeamMember.Name,
            Username=TeamMember.Username,
            Email=TeamMember.Email,
            PasswordHash=TeamMember.PasswordHash,
            HoursPerWeek=TeamMember.HoursPerWeek,
            Role=TeamMember.Role,
            Status=TeamMember.Status

        };



    public static TeamMemberEntity MapToDomain(this TeamMember TeamMember)
    
        => new TeamMemberEntity(TeamMember.Id, TeamMember.Name, TeamMember.Username, TeamMember.Email, TeamMember.PasswordHash, TeamMember.HoursPerWeek, TeamMember.Role, TeamMember.Status);

    public static void UpdateEntity(this TeamMemberEntity entity, TeamMember TeamMember)
    {
        TeamMember.Name=entity.Name;
        TeamMember.Username=entity.Username;
        TeamMember.Email=entity.Email;
        TeamMember.HoursPerWeek=entity.HoursPerWeek;
        TeamMember.Role=entity.Role;
        TeamMember.Status=entity.Status;

    }


    }