namespace TimeSheet.Application.Responses.TimeEntryResponse;


public class MonthTimeEntriesResponse
{
    public double TotalHours{get; set;}
    public List<DaySummaryResponse> Days{get; set; }=new();

}