using System.Data;
using TimeSheet.Domain.Exceptions;

namespace Timesheet.Domain.Entities;


public class ProjectModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public ProjectStatus ProjectStatus { get; set; }

    public Guid ClientId { get; set; }

    public Guid TeamMemberId { get; set; }

    public ProjectModel(Guid id, string name, string? description, ProjectStatus projectStatus, Guid id1, Guid id2)
    {

        Validate(name, id1, id2, projectStatus);
        Id = id;
        Name = name;
        Description = description;
        ProjectStatus = projectStatus;
        ClientId = id1;
        TeamMemberId = id2;

    }

    public void Update(string name, string? description, ProjectStatus projectStatus, Guid id1, Guid id2)
    {
        Validate(name, id1, id2, projectStatus);
        Name=name.Trim();
        Description=description?.Trim();
        ProjectStatus=projectStatus;
        ClientId=id1;
        TeamMemberId=id2;

    }




    public void Validate(string name, Guid clientId, Guid leadId, ProjectStatus status)
    {
            if(string.IsNullOrWhiteSpace(name))
                throw new DomainException("Project name is required");
            
            if(clientId==Guid.Empty)
                throw new DomainException("Client is required");

            if(leadId==Guid.Empty)
                throw new DomainException("Lead is required");

            if(!Enum.IsDefined(status))
                throw new DomainException("Status not defined");



    }
}
