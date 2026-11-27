using MediatR;
using TimeSheet.Application.Requests.RepostRequest;
using TimeSheet.Application.Responses.RepostResponses;

namespace TimeSheet.Application.Report.Queries.Getreport;


public record GetReportQuery(GetReportRequest Request): IRequest<ReportResponse>;