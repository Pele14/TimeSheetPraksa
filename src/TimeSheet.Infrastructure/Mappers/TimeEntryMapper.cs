using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Mappers;



public static class TimeEntryMapper
{
    public static TimeEntry MapToEntity(this TimeEntryEntity TimeEntry)
    
        => new TimeEntry
        {
            Id=TimeEntry.Id,
            Date=TimeEntry.Date,
            Time=TimeEntry.Time,
            Overtime=TimeEntry.Overtime,
            Description=TimeEntry.Description,
            CategoryId=TimeEntry.CategoryId,
            ProjectId=TimeEntry.ProjectId,
            TeamMemberId=TimeEntry.TeamMemberId

        };



    public static TimeEntryEntity MapToDomain(this TimeEntry TimeEntry)
    
        => new TimeEntryEntity(TimeEntry.Id, TimeEntry.Date, TimeEntry.Time, TimeEntry.Overtime, TimeEntry.Description, TimeEntry.CategoryId, TimeEntry.ProjectId, TimeEntry.TeamMemberId);

    public static void UpdateEntity(this TimeEntry TimeEntry, TimeEntryEntity entity)
    {
        TimeEntry.Date=entity.Date;
        TimeEntry.Time=entity.Time;
        TimeEntry.Overtime=entity.Overtime;
        TimeEntry.Description=entity.Description;
        TimeEntry.CategoryId=entity.CategoryId;
        TimeEntry.ProjectId=entity.ProjectId;
        TimeEntry.TeamMemberId=TimeEntry.TeamMemberId;

    }


    }