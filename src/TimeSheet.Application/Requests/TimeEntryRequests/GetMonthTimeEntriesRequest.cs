using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Application.Requests.TimeEntryRequests;

public class GetMonthTimeEntriesRequest
{
    [Range(2000, 2100)]
    public required int Year{get; set;}

    [Range(1, 12)]
    public required int Month{get; set;}


}