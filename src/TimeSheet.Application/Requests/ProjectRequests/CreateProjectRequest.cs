using System.IO.Compression;
using TimeSheet.Application.Client.Queries.GetClientById;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Requests.ProjectRequests;


public class CreateProjectRequest
{
    public required string Name{get; set;}
    public string? Description{get; set;}

    public required Guid ClientId{get; set;}

    public required Guid LeadID{get; set;}
}