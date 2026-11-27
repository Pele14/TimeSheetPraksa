namespace TimeSheet.Core.Reports;

public class ReportItem
{
    public DateOnly Date{get; set;}
    public string TeamMemberName{get; set;}=string.Empty;
    public string ClientName{get; set;}=string.Empty;
    public string ProjectName{get; set;}=string.Empty;
    public string CategoryName{get; set;}=string.Empty;
    public string? Description{get; set;}
    public double Time{get; set;}


}