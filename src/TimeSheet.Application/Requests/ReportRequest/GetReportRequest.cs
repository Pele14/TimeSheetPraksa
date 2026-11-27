namespace TimeSheet.Application.Requests.RepostRequest;


public class GetReportRequest
{
    public Guid? TeamMemberid{get; set;}
    public Guid? ClientId{get; set;}
    public Guid? ProjectId{get; set;}
    public Guid? CategoryId{get; set;}
    public DateOnly? StartDate{get; set;}
    public DateOnly? EndDate{get; set;}
}