using System.IO.Compression;
using TimeSheet.Application.Client.Queries.GetClientById;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Requests.ProjectRequests;


public class UpdateProjectRequest
{
    public required string Name{get; set;}
    public string? Description{get; set;}

    public ProjectStatus Status{get; set;}

    public required Guid ClientId{get; set;}

    public required Guid LeadID{get; set;}
}