

namespace TimeSheet.Application.Requests.TimeEntryRequests; 

public class UpdateTimeEntryRequest
{
    public required DateOnly Date{get; set;}
    public required double Hours{get; set;}
    public double? Overtime{get; set;}
    public string? Description{get; set;}
    public required Guid ProjectId{get; set;}
    public required Guid CategoryId{get; set;}
}