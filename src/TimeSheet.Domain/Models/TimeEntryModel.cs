
using TimeSheet.Domain.Exceptions;

namespace Timesheet.Domain.Entities;

public class TimeEntryModel
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public double Hours { get; set; }
    public double? Overtime { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid TeamMemberId { get; set; }

    public TimeEntryModel(Guid id, DateOnly date, double hours, double? overtime, string? description, Guid category, Guid project, Guid teammember)
    {
        Validate(date, hours, overtime, project, category);

        if(teammember==Guid.Empty)
            throw new DomainException("Team member is required");

        Id = id;
        Date = date;
        Hours = hours;
        Overtime = overtime;
        Description = description;
        CategoryId = category;
        ProjectId = project;
        TeamMemberId = teammember;

    }

    public void Update(DateOnly date, double hours, double? overtime, string? description, Guid projectId, Guid categoryId)
    {
        Validate(date, hours, overtime, projectId, categoryId);

        Date=date;
        Hours=hours;
        Overtime=overtime;
        Description=description?.Trim();
        ProjectId=projectId;
        CategoryId=categoryId;
    }



    private static void Validate(DateOnly date, double hours, double? overtime, Guid projectId, Guid categoryId)
    {
        if(date==default)
            throw new DomainException("Date is required");
        if(hours<=0 || hours>24)
            throw new DomainException("Hours must be between 0 and 24");
        if(overtime is<0)
            throw new DomainException("Overtime cannot be negative.");
        if(hours + (overtime ?? 0)>24)
            throw new DomainException("Total time cannot exceed 24 hours");
        if(projectId==Guid.Empty)
            throw new DomainException("Project is required");
        if(categoryId==Guid.Empty)
            throw new DomainException("Category is required");
    }
}
