

namespace TimeSheet.Application.Responses.TimeEntryResponse; 

public class TimeEntryResponse
{
    public Guid Id{get; set;}
    public DateOnly Date{get; set;}
    public double Hours{get; set;}
    public double? Overtime{get; set;}
    public string? Description{get; set;}
    public Guid ClientId{get; set;}
    public string ClientName{get; set;}=string.Empty;
    public Guid ProjectId{get; set;}
    public string ProjectName{get; set;}=string.Empty;
    public Guid CategoryId{get; set;}
    public string CategoryName{get; set;}=string.Empty;
    public Guid TeamMemberId{get; set;}
}