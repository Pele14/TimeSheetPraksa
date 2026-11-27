using System.Net.Quic;
using Microsoft.EntityFrameworkCore;
using TimeSheet.Core.Reports;
using TimeSheet.Core.Repositories;
using TimeSheet.Infrastructure.Configurations;
using TimeSheet.Infrastructure.Persistence;

namespace TimeSheet.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context)
    {
        _context=context;

    }

    public async Task<List<ReportItem>> GetReportAsync(ReportFilter filter)
    {
        var query=
            from te in _context.TimeEntries
            join p in _context.Projects.IgnoreQueryFilters([ProjectFilters.ActiveOnly]) on te.ProjectId equals p.Id
            join c in _context.Clients on p.ClientId equals c.Id
            join cat in _context.Categories on te.CategoryId equals cat.Id
            join tm in _context.TeamMembers on te.TeamMemberId equals tm.Id
            select new{te, p, c, cat, tm};

        if(filter.TeamMemberId.HasValue)
            query=query.Where(x=> x.te.TeamMemberId== filter.TeamMemberId.Value);
        if(filter.ClientId.HasValue)
            query=query.Where(x=> x.p.ClientId== filter.ClientId.Value);
        if(filter.CategoryId.HasValue)
            query=query.Where(x=> x.te.CategoryId==filter.CategoryId.Value);
        if(filter.ProjectId.HasValue)
            query=query.Where(x=> x.te.ProjectId==filter.ProjectId.Value);
        if(filter.StartDate.HasValue)
            query=query.Where(x=> x.te.Date>= filter.StartDate.Value);
        if(filter.EndDate.HasValue)
            query=query.Where(x=> x.te.Date<= filter.EndDate.Value);
        
        return await query
            .OrderBy(x=> x.te.Date)
            .ThenBy(x=> x.tm.Name)
            .Select(x=> new ReportItem
            {
                Date=x.te.Date,
                TeamMemberName=x.tm.Name,
                ClientName=x.c.Name,
                ProjectName=x.p.Name,
                CategoryName=x.cat.Name,
                Description=x.te.Description,
                Time=x.te.Hours +(x.te.Overtime ?? 0)
            })
            .ToListAsync();
    }
}