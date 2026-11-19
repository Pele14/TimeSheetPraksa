namespace Timesheet.Domain.Entities;


public class ProjectEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public ProjectStatus ProjectStatus { get; set; }

    public Guid ClientId { get; set; }

    public Guid TeamMemberId { get; set; }

    public ProjectEntity(Guid id, string name, string description, ProjectStatus projectStatus, Guid id1, Guid id2)
    {
        Id = id;
        Name = name;
        Description = description;
        ProjectStatus = projectStatus;
        ClientId = id1;
        TeamMemberId = id2;

    }
}
