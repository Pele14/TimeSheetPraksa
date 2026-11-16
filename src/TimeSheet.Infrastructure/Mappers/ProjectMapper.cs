using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Mappers;



public static class ProjectMapper
{
    public static Project MapToEntity(this ProjectEntity Project)
    
        => new Project
        {
            Id=Project.Id,
            Name=Project.Name,
            Description=Project.Description,
            ProjectStatus=Project.ProjectStatus,
            ClientId=Project.ClientId,
            TeamMemberId=Project.TeamMemberId
        };



    public static ProjectEntity MapToDomain(this Project Project)
    
        => new ProjectEntity(Project.Id, Project.Name, Project.Description, Project.ProjectStatus, Project.ClientId, Project.TeamMemberId);

    public static void UpdateEntity(this Project Project, ProjectEntity entity)
    {
        Project.Name=entity.Name;
        Project.Description=entity.Description;
        Project.ProjectStatus=entity.ProjectStatus;
        Project.ClientId=entity.ClientId;
        Project.TeamMemberId=entity.TeamMemberId;
    }


    }