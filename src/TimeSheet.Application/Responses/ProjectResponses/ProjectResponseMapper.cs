using Timesheet.Domain.Entities;
using TimeSheet.Application.Requests.ProjectResponses;

namespace TimeSheet.Application.Responses.ProjectResponses;
public static class ProjectResponseMapper
{
    public static ProjectResponse ToResponse(this ProjectEntity project, string clientName) 
    {
        return new ProjectResponse
        {
            Id=project.Id,
            Name=project.Name,
            Description=project.Description,
            Status=project.ProjectStatus,
            ClientId=project.ClientId,
            ClientName=clientName,
            LeadId=project.TeamMemberId

        };

    }

}