namespace TimeSheet.Application.Responses.RepostResponses;

public class ReportResponse
{
    public double Total{get; set; }
    public List<ReportItemResponse>Items{get; set;}=new();

}