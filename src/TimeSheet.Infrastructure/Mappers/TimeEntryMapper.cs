using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Mappers;



public static class TimeEntryMapper
{
    public static TimeEntry MapToEntity(this TimeEntryModel TimeEntry)

        => new TimeEntry
        {
            Id = TimeEntry.Id,
            Date = TimeEntry.Date,
            Hours = TimeEntry.Hours,
            Overtime = TimeEntry.Overtime,
            Description = TimeEntry.Description,
            CategoryId = TimeEntry.CategoryId,
            ProjectId = TimeEntry.ProjectId,
            TeamMemberId = TimeEntry.TeamMemberId

        };



    public static TimeEntryModel MapToDomain(this TimeEntry TimeEntry)

        => new TimeEntryModel(TimeEntry.Id, TimeEntry.Date, TimeEntry.Hours, TimeEntry.Overtime, TimeEntry.Description, TimeEntry.CategoryId, TimeEntry.ProjectId, TimeEntry.TeamMemberId);

    public static void UpdateEntity(this TimeEntryModel entity, TimeEntry TimeEntry)
    {
        TimeEntry.Date = entity.Date;
        TimeEntry.Hours = entity.Hours;
        TimeEntry.Overtime = entity.Overtime;
        TimeEntry.Description = entity.Description;
        TimeEntry.CategoryId = entity.CategoryId;
        TimeEntry.ProjectId = entity.ProjectId;
        TimeEntry.TeamMemberId = TimeEntry.TeamMemberId;

    }


}