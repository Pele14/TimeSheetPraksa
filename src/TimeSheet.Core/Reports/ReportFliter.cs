namespace TimeSheet.Core.Reports;


public class ReportFilter
{
    public Guid? TeamMemberId{get; set;}
    public Guid? ClientId{get; set;}
    public Guid? ProjectId{get; set;}
    public Guid? CategoryId{get; set;}
    public DateOnly? StartDate{get; set;}
    public DateOnly? EndDate{get; set;}
}