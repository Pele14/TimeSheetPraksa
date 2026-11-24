using Timesheet.Domain.Entities;

namespace TimeSheet.Application.Responses.TimeEntryResponse;
public static class TimeEntryResponseMapper
{
    public static TimeEntryResponse ToResponse(this TimeEntryModel timeEntry, ProjectModel project, ClientModel client, CategoryModel category) 
    {
        return new TimeEntryResponse
        {
            Id=timeEntry.Id,
            Date=timeEntry.Date,
            Hours=timeEntry.Hours,
            Overtime=timeEntry.Overtime,
            Description=timeEntry.Description,
            ClientId=client.Id,
            ClientName=client.Name,
            ProjectId=project.Id,
            ProjectName=project.Name,
            CategoryId=category.Id,
            CategoryName=category.Name,
            TeamMemberId=timeEntry.TeamMemberId
        };
    }

}