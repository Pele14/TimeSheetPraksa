using TimeSheet.Core.Reports;

namespace TimeSheet.Core.Repositories;

public interface IReportRepository
{
    Task<List<ReportItem>> GetReportAsync(ReportFilter filter);

}