
using MediatR;
using TimeSheet.Application.Report.Queries.Getreport;
using TimeSheet.Application.Responses.RepostResponses;
using TimeSheet.Core.Reports;
using TimeSheet.Core.Repositories;

public class GetReportHandler : IRequestHandler<GetReportQuery, ReportResponse>
{
    private readonly IReportRepository ReportRepository;

    public GetReportHandler(IReportRepository reportRepository)
    {
        ReportRepository=reportRepository;
    }
    public async Task<ReportResponse> Handle(GetReportQuery query, CancellationToken cancellationToken)
    {
        var request=query.Request;

        var filter=new ReportFilter
        {
            TeamMemberId=request.TeamMemberid,
            ClientId=request.ClientId,
            ProjectId=request.ProjectId,
            CategoryId=request.CategoryId,
            StartDate=request.StartDate,
            EndDate=request.StartDate
        };
        var items=await ReportRepository.GetReportAsync(filter);

        return new ReportResponse
        {
            Total=items.Sum(i=> i.Time),
            Items=items.Select(i=> new ReportItemResponse
            {
                Date=i.Date,
                TeamMemberName=i.TeamMemberName,
                ClientName=i.ClientName,
                ProjectName=i.ProjectName,
                CategoryName=i.CategoryName,
                Description=i.Description,
                Time=i.Time
            }).ToList()
        };
    }
}