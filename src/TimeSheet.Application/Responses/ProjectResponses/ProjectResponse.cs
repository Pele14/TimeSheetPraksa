using System.IO.Compression;
using TimeSheet.Domain.Enums;

namespace TimeSheet.Application.Requests.ProjectResponses;


public class ProjectResponse
{
    public required Guid Id{get; set;}
    public required string Name{get; set;}
    public string? Description{get; set;}
    public required ProjectStatus Status{get; set;}
    public required Guid  ClientId{get; set;}

    public required string ClientName{get; set;}
    public required Guid LeadId{get; set;}

}