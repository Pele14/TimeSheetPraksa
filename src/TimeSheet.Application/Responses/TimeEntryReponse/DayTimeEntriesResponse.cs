namespace TimeSheet.Application.Responses.TimeEntryResponse;

public class DayTimeEntriesResponse
{
    public DateOnly Date{get; set; }
    public double TotalHours{get; set;}
    public List<TimeEntryResponse> TimeEntries{get; set; }=new();

}